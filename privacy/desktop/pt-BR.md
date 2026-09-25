[Languages](README.md)

# Política de privacidade

Última atualização: 25 de setembro de 2026

**BUtil** by Siarhei Kuchuk

Nome do aplicativo: BUtil
Nome do desenvolvedor: Siarhei Kuchuk

O BUtil faz backup, sincroniza e restaura arquivos neste computador. Ele também pode importar mídia, compartilhar uma pasta ou enviar arquivos para um servidor que você configurar. Ele não cria uma conta de desenvolvedor. O desenvolvedor não opera um servidor que receba seus arquivos, senhas ou dados de uso.

## Dados que o desenvolvedor não coleta

O aplicativo não inclui anúncios, análise, relatórios de falhas nem SDKs de rastreamento. O desenvolvedor não coleta, não vende e não compartilha dados pessoais.

## Dados armazenados no seu computador

### Tarefas e configurações

As definições de tarefas ficam apenas neste computador. Uma tarefa pode incluir caminhos de pastas, um agendamento, configurações de armazenamento e as senhas ou tokens que você digitar. Senhas e segredos de armazenamento são criptografados neste computador antes de serem salvos e só podem ser lidos neste computador. Esses valores não são enviados ao desenvolvedor.

- Tarefas do Windows: `%AppData%\BUtil Backup Tasks`
- Tarefas do Linux: `~/.config/BUtil Backup Tasks`
- Configurações do Windows (incluindo o tema e o idioma escolhido por último para a licença ou a privacidade): `%AppData%\BUtil\Settings\v1`
- Configurações do Linux: `~/.config/BUtil/Settings/v1`
- Estado das tarefas do Windows: `%AppData%\BUtil\States`
- Estado das tarefas do Linux: `~/.config/BUtil/States`
- Estado da importação de mídia do Windows: `%AppData%\BUtil Backup Tasks - States`
- Estado da importação de mídia do Linux: `~/.config/BUtil Backup Tasks - States`

### Arquivos que você escolhe

Backup, sincronização, restauração e importação leem e gravam as pastas que você selecionar. Esses arquivos permanecem neste computador ou no destino de armazenamento que você configurar. O aplicativo não os envia ao desenvolvedor.

### Registros

Os registros de diagnóstico são gravados apenas neste computador:

- Windows: `%LocalAppData%\BUtil\logs\v4`
- Linux: `~/.local/share/BUtil/logs/v4`

Esses arquivos não são enviados a lugar nenhum.

Nenhum servidor do desenvolvedor é usado para armazenar seus dados.

## Uso da rede

### Destinos que você configura

Quando uma tarefa é executada, o aplicativo se conecta apenas ao local que você definir. Pode ser uma pasta local ou um servidor que você informar: FTP, FTPS, SFTP, WebDAV, SMB, NFS, armazenamento compatível com S3 ou Azure Blob Storage. Nomes de arquivos, conteúdo e as credenciais que você informou são enviados a esse servidor para que a tarefa possa ser executada. Cada um desses serviços tem a própria política de privacidade. O desenvolvedor não recebe esse tráfego.

O BUtil Server pode escutar neste computador para que um cliente BUtil que você configurar possa enviar arquivos. Esse tráfego fica entre os computadores que você configurar.

### Verificação de atualizações

Compilações que não são da Store podem solicitar a versão mais recente no GitHub:

`https://api.github.com/repos/drweb86/butil/releases/latest`

O GitHub (Microsoft) recebe uma solicitação HTTPS normal (endereço IP, user-agent, horário). O desenvolvedor não recebe esse tráfego.

Instalações da Microsoft Store não usam essa verificação; a Store entrega as atualizações.

### Links que você abre

O aplicativo pode abrir estas páginas no navegador do sistema. Esses sites têm as próprias políticas de privacidade:

- Página do projeto: [github.com/drweb86/butil](https://github.com/drweb86/butil)
- Versão mais recente: [github.com/drweb86/butil/releases/latest](https://github.com/drweb86/butil/releases/latest)
- Ajuda de padrões de arquivo: [learn.microsoft.com file globbing](https://learn.microsoft.com/en-us/dotnet/core/extensions/file-globbing#pattern-formats)
- Ajuda de formato de data: [learn.microsoft.com custom date and time format strings](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)
- Créditos dos ícones: [github.com/drweb86/butil Icons](https://github.com/drweb86/butil/blob/master/help/Icons.md)

A licença e esta política de privacidade são mostradas dentro do aplicativo. Elas não são abertas como páginas da web.

## Agendamento

No Windows, você pode executar uma tarefa ao entrar ou em um agendamento semanal. O aplicativo registra isso no Agendador de Tarefas do Windows com um nome que começa com `BUtil`. Isso apenas inicia este aplicativo no seu computador.

## Crianças

O aplicativo é uma ferramenta de backup e sincronização de arquivos. Ele não é dirigido a crianças menores de 13 anos.

## Terceiros

O GitHub processa a solicitação de verificação de atualizações e as páginas que você abre, conforme descrito acima. A Microsoft Store processa instalações e atualizações da Store. Os provedores de armazenamento que você configura processam os arquivos e as credenciais que a tarefa envia a eles. O desenvolvedor não recebe esse tráfego.

## Alterações

As atualizações desta política serão publicadas neste arquivo no repositório do projeto.

## Contato

Nome do aplicativo: BUtil
Nome do desenvolvedor: Siarhei Kuchuk

Perguntas: [github.com/drweb86/butil/issues](https://github.com/drweb86/butil/issues)
