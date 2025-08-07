using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cSys22;
using System.Runtime.InteropServices;

namespace Sage50c.WebAPI.Services
{
    public class Sage50cApiService
    {
        private SystemSettings? _systemSettings;
        private DSOFactory? _dsoCache;
        private BSOItemTransaction? _bsoItemTransaction;
        private bool _isInitialized = false;

        public SystemSettings? SystemSettings => _systemSettings;
        public DSOFactory? DSOCache => _dsoCache;
        public BSOItemTransaction? BsoItemTransaction => _bsoItemTransaction;
        public bool IsInitialized => _isInitialized;

        public bool Initialize(string api, string companyId, bool debugMode = false)
        {
            try
            {
                APIEngine.Initialize(api, companyId, debugMode);
                
                _systemSettings = APIEngine.SystemSettings;
                _dsoCache = APIEngine.DSOCache;
                
                // Não inicializar BSOItemTransaction aqui - deixar para quando necessário
                // O erro COM acontece durante a inicialização
                
                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inicializar API Sage50c: {ex.Message}", ex);
            }
        }

        public BSOItemTransaction GetBSOItemTransaction()
        {
            if (_bsoItemTransaction == null)
            {
                _bsoItemTransaction = new BSOItemTransaction();
                // Não definir UserPermissions aqui - será definido apenas quando necessário
                // como no sample (SetUserPermissions método)
            }
            return _bsoItemTransaction;
        }

        public void SetUserPermissions(BSOItemTransaction bsoItemTransaction)
        {
            // Definir UserPermissions apenas quando necessário, como no sample
            bsoItemTransaction.UserPermissions = _systemSettings!.User;
            bsoItemTransaction.PermissionsType = S50cSys22.FrontOfficePermissionEnum.foPermByUser;
        }

        // Executar operações COM em thread STA para resolver problemas de interface
        public T ExecuteInSTA<T>(Func<T> operation)
        {
            T result = default(T)!;
            Exception? thrownException = null;

            var thread = new Thread(() =>
            {
                try
                {
                    result = operation();
                }
                catch (Exception ex)
                {
                    thrownException = ex;
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            if (thrownException != null)
            {
                throw thrownException;
            }

            return result;
        }

        public void Terminate()
        {
            if (_isInitialized && APIEngine.APIInitialized)
            {
                // Limpar referências como no sample
                _bsoItemTransaction = null;
                
                APIEngine.Terminate();
                _isInitialized = false;
            }
        }
    }
}