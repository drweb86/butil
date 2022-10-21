md5

# Usage
``` md5 'list of arguments separated with blank' ```

# Command Line Arguments:

## -Help
Outputs localized help about parameters

## -Dev
Counts md5 for some packer components

## -md5 "Filename"
Counts md5 of file and outputs it to console

## -Verify "Filename"
Checks if md5 of file and md5 stored in "Filename".md5 are equal

## -Verify "Filename" "FileWithMd5"
Checks if md5 of file and md5 stored in "FileWithMd5" are equal

## -Sign "Filename"
Produces "Filename".md5 with md5 inside of "Filename"

## -Sign "Filename" "FileWithMd5"
Produces "FileWithMd5" with md5 inside of "Filename"
