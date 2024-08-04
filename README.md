## ChartOfAccountsApi - README

### Descrição

O ChartOfAccountsApi é uma API RESTful desenvolvida em c# .NET 8 para
dar mais agilidade no controle de plano de contas. A API permite a listagem, inclusão e exclusão de contas, além de um serviço que sugere o próximo código da conta que está sendo cadastrada.

### Pré-requisitos

* **SQL Server:** Você precisará de um servidor SQL Server em execução para armazenar os dados.
* **.NET 8 SDK:** Certifique-se de ter o .NET 8 SDK instalado em sua máquina. Você pode baixá-lo em: [https://dotnet.microsoft.com/pt-br/download/dotnet/8.0](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)

### Configuração

1. **Crie o banco de dados:**
   - Execute o script SQL localizado em `/scripts` no seu servidor SQL Server para criar a estrutura do banco de dados.

2. **Configure a string de conexão:**
   - Abra o arquivo `appsettings.json` localizado em `/src/DiegoMoreno.ChartOfAccountsApi.Api`.
   - Substitua o valor da chave `ConnectionStrings:ChartOfAccountsApiDbSqlServer` pela string de conexão do seu banco de dados recém-criado.

### Executando a aplicação

1. **Abra um terminal:** Navegue até o diretório do projeto principal (`/src/DiegoMoreno.ChartOfAccountsApi.Api`).

2. **Execute o comando:**
   ```bash
   dotnet run
   ```
   A aplicação iniciará e exibirá a URL em que está sendo executada (ex: `Now listening on: https://https://localhost:7248`).

### Acessando a API

Você tem duas opções para interagir com a API:

**Opção 1: Navegador**

1. Abra a URL da aplicação no seu navegador.
2. Acesse a documentação da API em: `/swagger/index.html`.
3. Utilize a interface do Swagger para testar os endpoints, seguindo as instruções da documentação.

**Opção 2: Postman/Insomnia**

1. Importe a collection do Postman localizada em `/collection/ChartOfAccountsApi.postman_collection.json`.
2. Atualize a URL base em todos os endpoints para a URL em que sua aplicação está sendo executada (a que você obteve na etapa "Executando a aplicação").
3. Utilize o Postman ou Insomnia para enviar requisições para os endpoints da API.

---

**Observações:**

* Este projeto segue as melhores práticas RESTful para o design de APIs.
* A documentação da API no Swagger fornece informações detalhadas sobre os endpoints, parâmetros e exemplos de uso.
* Sinta-se à vontade para explorar o código-fonte e os testes unitários para entender melhor a implementação.

**Contribuindo:**

Contribuições são bem-vindas! Se você encontrar algum problema ou tiver sugestões de melhoria, por favor, abra uma issue ou envie um pull request.