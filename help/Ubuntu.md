# Build instruction for Ubuntu 23.10

1. Navigate to 
https://github.com/drweb86/butil/releases/latest

2. Download in Assets 
Source code(zip)

3. Extract archive to any folder

4. Go to this folder

5. Go to subfolder sources

6. Click in Address Bar ... and Open Terminal

7. Install .Net SDK 8

```
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

8. Build

Template command:
```dotnet publish /p:Version=###VERSION### /p:AssemblyVersion=###VERSION### -c Release --self-contained true -o bin-###VERSION###```

Instead of `###VERSION###`, put release name from which you've downloaded the sources, e.g. `2024.01.10`.

Result command example:
```dotnet publish /p:Version=2024.01.10 /p:AssemblyVersion=2024.01.10 -c Release --self-contained true -o bin-2024.01.10```

9. Install dependencies

```sudo apt-get install -y 7zip```

10. Command line tool:

Open folder template
```bin-###VERSION###```

Example folder:
```bin-2024.01.10```

See files
```butilc```
```butil-ui.Desktop```