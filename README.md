## ChartOfAccountsApi - LEIA-ME

### Descrição

O ChartOfAccountsApi é uma API RESTful desenvolvida em c# .NET 8 para
dar mais agilidade no controle de plano de contas. A API permite a listagem, inclusão e exclusão de contas, além de um serviço que sugere o próximo código da conta que está sendo cadastrada.

### Tecnologias Utilizadas
* **Backend**: C# .NET 8, ASP.NET Core
* **Banco de Dados**: SQL Server
* **Documentação da API**: Swagger

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

**Orientações:**

* Os tipos de conta devem ser criados a partir do banco de dados, conforme as instruções de configuração na etapa 1.

* Ao começar, crie uma nova conta.

    * Para isso, execute o endpoint `/v1/accountTypes` para retornar todos os tipos de conta e obter o ID do tipo de conta desejado.
    * Agora execute o endpoint POST `/v1/accounts`.
    * Nesse primeiro momento, vamos criar uma conta que poderá ser pai.
    * Preencha o JSON do corpo da requisição com as seguintes informações:
```JSON
{
"codeGoup": "1",
"name": <Coloque aqui um nome para identificar sua conta>,
"idAccountType": <Coloque aqui o Id obtido no endpoint /v1/accountTypes>,
"acceptEntries": false <- As contas que podem ser pai não podem aceitar lançamentos
}
```

* Agora vamos usar o endpoint para sugerir o próximo código da conta.

    * Para executar esse endpoint, você precisa do ID da conta criada no POST anterior.
    * Execute o endpoint GET `/v1/accounts/nextCode/<coloque aqui o Id da conta criada no post anterior>`.
    * Ao executar o endpoint, você terá um retorno parecido com este JSON:
```JSON
{
  "data": {
    "suggestedCode": "2.3.5",
    "newParent": null
  },
  "message": "string"
}
```
    * Nesse endpoint, quando não for possível gerar um novo código para o pai atual, ele sugerirá um novo código para um pai diferente.
    * Quando isso ocorre, o campo newParent virá preenchido com o ID (GUID) da conta pai sugerida.

* Agora vamos criar uma nova conta que será filha da conta já criada e também utilizaremos o código sugerido.

    * Aqui você deverá executar o endpoint POST `/v1/accounts`, conforme as orientações anteriores. A mudança será no corpo da requisição, que você deverá preencher com um JSON parecido com este:
```JSON
{
"idParentAccount": <Coloque aqui o Id (Guid) da primeira conta criada>,
"codeGoup": "<Envie aqui o suggestedCode obtido no endpoint que sugere o próximo código>",
"name": <Coloque aqui um nome para identificar sua conta>,
"idAccountType": <Coloque aqui o Id obtido no endpoint /v1/accountTypes>,
"acceptEntries": <Aqui você poderá colocar true ou false>
}
```

* Agora vamos listar as contas com grupo de códigos.

    * Para isso, basta executar o endpoint GET `/v1/accounts/listWithCodeGroup`.
    * Nesse endpoint, você tem a opção de filtrar as informações para retornar somente contas que são pais e também tem a paginação, basta enviar na query os parâmetros: `onlyParents=<É um bool, você poderá informar true ou false>, pageNumber=<Informe o número da página atual>, pageSize=<Informe o tamanho da página desejado>`.
    * Lembre-se de que todos os parâmetros são opcionais. O valor padrão de `onlyParents` é false, `pageNumber` quando não informado será 1 e `pageSize` será 25.
    * O retorno será um JSON com todas as contas já criadas.
    * No JSON da resposta, você também tem acesso às informações da paginação, como: `total`, `totalPages`, `currentPage` e `pageSize`.

* Agora vamos deletar uma conta.

    * Aqui você deverá ter o ID da conta, que pode ser obtido através do endpoint que lista as contas com grupo de códigos.
    * Execute o endpoint DELETE `/v1/accounts/<Informe aqui o Id da conta que será deletada>`.
    * Você receberá o status code 200 quando a conta for deletada.
    * Quando não for possível deletar a conta, você receberá o status code correspondente.
    * Todas as respostas contêm um JSON com mais informações.
    * A conta que é pai e está atribuída a uma conta filha não poderá ser excluída.

---
### Estrutura do Projeto

O projeto está organizado da seguinte forma:

* `/src/DiegoMoreno.ChartOfAccountsApi.Api`: Contém a API principal, controladores, modelos, configurações, etc.
* `/src/DiegoMoreno.ChartOfAccountsApi.Application`: Contém o padrão application service para intermediações em a camada de apresentação e a camada de domínio, fazendo a orquestração dos serviços e repositórios do domínio.
* `/src/DiegoMoreno.ChartOfAccountsApi.Domain`: Contém as entidades de domínio, interfaces de repositório e serviços de domínio.
* `/src/DiegoMoreno.ChartOfAccountsApi.Infra.Data`: Contém a implementação dos repositórios e o contexto do banco de dados.
* `/tests/DiegoMoreno.ChartOfAccountsApi.Tests`: Contém os testes de unidade
* `/scripts`: Contém o script SQL para criar a estrutura do banco de dados.
* `/collection`: Contém a collection do Postman para testar a API.

---

### Observações

* Este projeto segue as melhores práticas RESTful para o design de APIs.
* A documentação da API no Swagger fornece informações detalhadas sobre os endpoints, parâmetros e exemplos de uso.
* Sinta-se à vontade para explorar o código-fonte e os testes unitários para entender melhor a implementação.

### Contribuindo

Contribuições são bem-vindas! Se você encontrar algum problema ou tiver sugestões de melhoria, por favor, abra uma issue ou envie um pull request.

### Contato

* **Diego Moreno**
* **Email:** dierudito@gmail.com