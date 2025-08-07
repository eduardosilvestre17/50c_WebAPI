# Sage50c Web API

API REST para integração com Sage50c focada em Documentos de Venda.

## Funcionalidades

### Endpoints Disponíveis

#### 1. Gestão da API
- `POST /api/api/initialize` - Inicializa a API Sage50c
- `GET /api/api/status` - Verifica o status da API
- `POST /api/api/terminate` - Termina a API
- `GET /api/api/company` - Obtém informações da empresa

#### 2. Documentos de Venda
- `GET /api/documentosvenda` - Lista documentos de venda
- `POST /api/documentosvenda` - Cria novo documento de venda
- `DELETE /api/documentosvenda/{transSerial}/{transDocument}/{transDocNumber}` - Elimina documento de venda

## Como Usar

### 1. Inicializar a API

Primeiro, inicialize a API Sage50c:

```http
POST /api/api/initialize
Content-Type: application/json

{
  "api": "CRTL",
  "companyId": "MEGAPC",
  "debugMode": true
}
```

### 2. Verificar Status

```http
GET /api/api/status
```

### 3. Listar Documentos de Venda

```http
GET /api/documentosvenda?pageSize=10&page=1
```

Parâmetros opcionais:
- `pageSize`: Número de registros por página (padrão: 50)
- `page`: Número da página (padrão: 1)  
- `startDate`: Data inicial (formato: yyyy-MM-dd)
- `endDate`: Data final (formato: yyyy-MM-dd)

### 4. Criar Documento de Venda

```http
POST /api/documentosvenda
Content-Type: application/json

{
  "transDocument": "FT",
  "transSerial": "A",
  "partyID": 1,
  "createDate": "2023-12-01",
  "currencyID": "EUR",
  "taxIncluded": true,
  "tenderID": 1,
  "paymentID": 1,
  "comments": "Documento criado via API",
  "details": [
    {
      "itemID": "ART001",
      "description": "Artigo de teste",
      "quantity": 2,
      "unitPrice": 10.50,
      "taxIncludedPrice": 12.88,
      "unitOfSaleID": "UN",
      "warehouseID": 1,
      "taxPercent": 23
    }
  ]
}
```

### 5. Eliminar Documento de Venda

```http
DELETE /api/documentosvenda/A/FT/1
```

## Configuração

### appsettings.json

```json
{
  "Sage50c": {
    "DefaultApi": "CRTL",
    "DefaultCompanyId": "MEGAPC", 
    "DebugMode": true
  }
}
```

## Requisitos

- .NET 8.0
- Sage50c instalado
- Interops do Sage50c (incluídos na pasta Interops/)

## Instalação e Execução

1. Certifique-se de que tem o Sage50c instalado
2. Coloque os ficheiros Interops na pasta `Interops/`
3. Execute o comando:
   ```bash
   dotnet run
   ```
4. A API estará disponível em `https://localhost:7000` (ou porta configurada)
5. O Swagger UI estará disponível na raiz: `https://localhost:7000`

## Tratamento de Erros

A API inclui middleware de tratamento de erros que retorna respostas padronizadas:

```json
{
  "success": false,
  "message": "Descrição do erro",
  "data": null
}
```

## Logging

Os logs são configurados para diferentes níveis:
- `Information`: Operações normais
- `Warning`: Avisos da aplicação
- `Error`: Erros e exceções
- `Debug`: Informações detalhadas (apenas em desenvolvimento)

## Notas Importantes

- **Plataforma x86**: A API deve ser executada em modo x86 devido aos Interops do Sage50c
- **EmbedInteropTypes**: Todos os Interops estão configurados com `EmbedInteropTypes="False"` para compatibilidade
- **Licença**: O tipo de licença configurado é "CRTL" (conforme especificado no seu sistema)
- **CompanyID**: Configurado para "MEGAPC" (conforme o seu ambiente)
- **Inicialização**: A inicialização da API é obrigatória antes de usar os endpoints de documentos
- **Instalação Sage50c**: Certifique-se de que o Sage50c está instalado e configurado corretamente
- **Regras de Negócio**: Os documentos são criados seguindo as regras de negócio do Sage50c

### Configurações Específicas dos Interops

Se encontrar problemas com os Interops, verifique se:
1. Todos os ficheiros .dll estão na pasta `Interops/`
2. Todos têm `EmbedInteropTypes="False"` no projeto
3. A plataforma de compilação está definida para x86
4. O Sage50c está instalado na mesma máquina# 50c_WebAPI
