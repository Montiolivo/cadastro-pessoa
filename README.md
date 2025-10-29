# 🧾 CadastroPessoa API

API desenvolvida em **.NET 8** para gerenciar o cadastro de pessoas.  
Possui autenticação via **JWT (JSON Web Token)**, armazenamento em **banco de dados em memória (InMemoryDatabase)** e está **hospedada no Azure App Service**.  
Inclui testes automatizados escritos com **xUnit**.

---

## 🌐 API publicada

A API está disponível no seguinte endereço com Swagger UI:  

[https://cadastropessoaapi-eceef3euh4e0hdbs.brazilsouth-01.azurewebsites.net/swagger](https://cadastropessoaapi-eceef3euh4e0hdbs.brazilsouth-01.azurewebsites.net/swagger)

> ⚠️ No Swagger, você encontrará **duas versões da API**:
> - **v1** → versão inicial da API para cadastro de pessoas.  
> - **v2** → versão atualizada com endereço.
---

## 🚀 Funcionalidades

- 🔐 **Autenticação JWT** (login e geração de token)  
- 👤 **Cadastro de pessoas** (CRUD completo)  
- 🧠 **Banco em memória** para armazenamento dos dados  
- 🧪 **Testes automatizados com xUnit**  
- 🌩️ **Hospedagem no Azure App Service**  
- 📘 **Swagger UI** para documentação e teste de endpoints  

---

## ⚙️ Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)  
- **Entity Framework Core (InMemory)**  
- **ASP.NET Core Web API**  
- **JWT (System.IdentityModel.Tokens.Jwt)**  
- **Swagger / Swashbuckle**  
- **xUnit** para testes unitários  
- **Azure App Service** para hospedagem  

---

## 📁 Estrutura do Projeto

Cadastro_Pessoa/
├── Controllers/
│ ├── AuthController.cs
│ └── PessoaController.cs
├── Data/
│ └── DataContext.cs
├── Models/
│ ├── Pessoa.cs
│ └── Usuario.cs
├── Tests/
│ ├── Controllers/
│ │ └── PessoaControllerTests.cs
│ └── Mocks/
│ └── MockData.cs
├── Program.cs
└── appsettings.json


---

## 🔐 Autenticação

A autenticação é feita via **JWT Token**.  
Para obter o token, envie um `POST` para o endpoint:


### 🔸 Corpo da Requisição
```json
{
  "username": "admin",
  "password": "123456"
}
```
### 🔸 Resposta Esperada
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6..."
}
```


