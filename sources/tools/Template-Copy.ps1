Param(
    [string]$TemplateFilePath,   # Path to the template file
    [string]$DestinationFilePath, # Path to the destination file where the new file will be saved
    [hashtable]$Replacements      # Hashtable of replacements in the format @{ 'placeholder1' = 'value1'; 'placeholder2' = 'value2' }
)

# Function to copy the template file and replace arguments
function Copy-And-ReplaceTemplate {
    param (
        [string]$TemplateFilePath,
        [string]$DestinationFilePath,
        [hashtable]$Replacements
    )
    
    # Check if the template file exists
    if (-not (Test-Path -Path $TemplateFilePath)) {
        Write-Error "Template file '$TemplateFilePath' does not exist."
        return
    }

    # Read the content of the template file
    try {
        $content = Get-Content -Path $TemplateFilePath -Raw
    } catch {
        Write-Error "Failed to read the template file: $_"
        return
    }
    
    # Loop through each replacement and replace the placeholders with the corresponding values
    foreach ($key in $Replacements.Keys) {
        $value = $Replacements[$key]
        $placeholder = "${key}"  # Assuming placeholders are in the format {{placeholder}}
        
        Write-Host "Replacing '$placeholder' with '$value'"
        $content = $content -replace [regex]::Escape($placeholder), $value
    }
    
    # Write the new content to the destination file
    try {
        Set-Content -Path $DestinationFilePath -Value $content
        Write-Host "File successfully created at '$DestinationFilePath'"
    } catch {
        Write-Error "Failed to write to the destination file: $_"
    }
}

# Call the function
Copy-And-ReplaceTemplate -TemplateFilePath $TemplateFilePath -DestinationFilePath $DestinationFilePath -Replacements $Replacements