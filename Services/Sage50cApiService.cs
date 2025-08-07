using S50cBL22;
using S50cBO22;
using S50cDL22;
using S50cSys22;

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
                
                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao inicializar API Sage50c: {ex.Message}", ex);
            }
        }

        public void Terminate()
        {
            if (_isInitialized && APIEngine.APIInitialized)
            {
                APIEngine.Terminate();
                _isInitialized = false;
            }
        }
    }
}