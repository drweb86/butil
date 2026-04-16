# Ubuntu

There're several installation methods.

## Method 1. Installation via APT Repository (best)

This method lets you install and update BUtil using standard `apt` commands.

### One-time setup

Click copy and paste in the terminal.

```
curl -fsSL https://drweb86.github.io/butil/gpg-key.pub | sudo gpg --dearmor -o /usr/share/keyrings/butil.gpg
echo "deb [arch=$(dpkg --print-architecture) signed-by=/usr/share/keyrings/butil.gpg] https://drweb86.github.io/butil stable main" | sudo tee /etc/apt/sources.list.d/butil.list > /dev/null
```

### Install

```
sudo apt update && sudo apt install butil
```

### Update

```
sudo apt update && sudo apt upgrade butil
```

### Uninstall

```
sudo apt remove butil
sudo rm /etc/apt/sources.list.d/butil.list /usr/share/keyrings/butil.gpg
```

User configuration in `~/.config/BUtil` is preserved after uninstall. Remove it manually if needed.

## Method 2. Installation via .deb Download

Download the `.deb` file for your architecture from the [latest release](https://github.com/drweb86/butil/releases/latest) and install:

```
sudo dpkg -i butil_*_amd64.deb
sudo apt-get install -f
```

For ARM64:

```
sudo dpkg -i butil_*_arm64.deb
sudo apt-get install -f
```

### Uninstall

```
sudo apt remove butil
```

## Method 3. Installation via Bash script

Open terminal, paste

```
wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-install.sh | bash
```

for preview version

```
wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-install.sh | bash -s -- --latest
```

### Uninstallation (source install only)

```
wget -O - https://raw.githubusercontent.com/drweb86/butil/master/sources/ubuntu-uninstall.sh | bash
```

## Executables

After installation (APT or .deb), the following commands are available:

- **`butil-ui`** — graphical user interface
- **`butilc`** — console tool for automation and scheduling
