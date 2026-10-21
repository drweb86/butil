[Languages](README.md)

# Política de privacidade

Última atualização: 21 de outubro de 2026

**BUtil** by Siarhei Kuchuk

Nome do aplicativo: BUtil
Nome do desenvolvedor: Siarhei Kuchuk

O BUtil faz backup, sincroniza e restaura arquivos neste dispositivo. Ele também pode importar mídia, compartilhar uma pasta por FTPS ou enviar arquivos para um servidor que você configurar. Ele não cria uma conta do desenvolvedor. O desenvolvedor não opera um servidor que receba seus arquivos, senhas ou dados de uso.

## Dados que o desenvolvedor não coleta

O aplicativo não inclui anúncios, análise, relatórios de falhas nem SDKs de rastreamento. O desenvolvedor não coleta, não vende e não compartilha dados pessoais.

Esta política de privacidade e a licença são mostradas dentro do aplicativo. Elas não são abertas como páginas da web.

## Dados armazenados no seu dispositivo

### Tarefas e configurações

As definições das tarefas são armazenadas apenas no armazenamento privado do aplicativo neste dispositivo. Uma tarefa pode incluir caminhos de pastas, um agendamento, configurações de armazenamento e as senhas ou tokens que você digitar. As senhas e os segredos de armazenamento são selados com o repositório de chaves do Android antes de serem salvos e só podem ser lidos neste dispositivo. Esses valores não são enviados ao desenvolvedor. Eles não são incluídos no backup do Android. Desinstalar o aplicativo os remove.

O tema que você escolhe é armazenado nas configurações privadas do aplicativo neste dispositivo.

### Arquivos que você escolhe

Backup, sincronização, restauração, importação e envio leem e gravam as pastas que você seleciona com o seletor de documentos do sistema, ou o servidor que você configura. Esses arquivos permanecem nas pastas escolhidas ou no destino de armazenamento que você configura. O aplicativo não os envia ao desenvolvedor. Desinstalar o aplicativo não exclui os arquivos das pastas que você selecionou.

### Certificado FTPS

Quando você compartilha uma pasta por FTPS, o aplicativo cria um certificado autoassinado no armazenamento privado do aplicativo. Desinstalar o aplicativo remove esse certificado. Os arquivos compartilhados permanecem na pasta que você autorizou.

Nenhum servidor do desenvolvedor é usado para armazenar seus dados.

## Uso da rede

### Destinos que você configura

Quando uma tarefa é executada, o aplicativo se conecta apenas ao local que você definiu. Pode ser uma pasta neste dispositivo ou um servidor que você informar: FTP, FTPS, SFTP, WebDAV, SMB, NFS, armazenamento compatível com S3 ou Azure Blob Storage. Os nomes dos arquivos, o conteúdo dos arquivos e as credenciais que você inseriu são enviados a esse servidor para que a tarefa possa ser executada. Cada um desses serviços tem a própria política de privacidade. O desenvolvedor não recebe esse tráfego.

Compartilhar uma pasta por FTPS escuta neste dispositivo para que um cliente FTPS que você configure possa ler ou gravar a pasta que você autorizou, usando o nome de usuário e a senha que você definiu. Esse tráfego permanece entre os seus dispositivos (e qualquer pessoa na rede que tenha essas credenciais). O desenvolvedor não é parte dele. Você é responsável por quem pode alcançar a porta, por qual pasta você compartilha e pela força dessa senha.

### Link que você abre

A linha da versão no menu pode abrir a página inicial do projeto no navegador do sistema:

[github.com/drweb86/butil](https://github.com/drweb86/butil)

Esse site tem a própria política de privacidade. O desenvolvedor não recebe esse tráfego.

O aplicativo não verifica atualizações e não entra em contato com um serviço de atualização.

## Agendamento

Você pode executar uma tarefa quando o dispositivo iniciar ou nos dias que escolher. O aplicativo registra isso no despertador do sistema. Isso apenas inicia este aplicativo no seu dispositivo.

## Permissões

O aplicativo usa somente as seguintes permissões:

- **Internet** e **estado da rede**, para alcançar um servidor que você configure e para compartilhar uma pasta por FTPS.
- **Notificações** e um **serviço em primeiro plano**, para manter uma tarefa em execução com a tela desligada.
- **Alarmes exatos** e **execução na inicialização**, para iniciar uma tarefa agendada.
- **Bloqueio de ativação**, para reduzir a suspensão durante uma transferência.

## Crianças

O aplicativo é uma ferramenta de backup e sincronização de arquivos. Ele não se dirige a crianças menores de 13 anos.

## Terceiros

Os provedores de armazenamento que você configura processam os arquivos e as credenciais que a tarefa envia a eles. A abertura da página inicial do projeto é feita pelo navegador do sistema e por esse site. O desenvolvedor não recebe esse tráfego.

## Alterações

As atualizações desta política estão incluídas no aplicativo.

## Contato

Nome do aplicativo: BUtil
Nome do desenvolvedor: Siarhei Kuchuk

Perguntas sobre este aplicativo para Android podem ser enviadas pela ficha da loja usada para instalá-lo ou em [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues).
