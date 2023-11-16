# Photo handler
## Motivation
I used to copy photos from my phone to my pc to have a backup.
I did not delete these photos from my phone, so they would be copied again at the next backup.
Therefore I have many copies of the same images.
Also I made the directory names and would like it categorized by year.
Example of the 3 instances of the same picture:
```bash
SourceFolder
    ├─ picture.jpg
    ├─ Subfolder1
    │   └─ picture.jpg
    └─ Subfolder2
        └─ Subsubfolder
            └─ picture.jpg
```
Which this program would turn into:
```bash
DestinationFolder
    └─ 2020
        └─ picture.jpg
```
Assuming the picture was created in 2020.

## What it does
It takes a path (src dir), and from this path it finds all files.

Then the program asks for some criteria by which to search for duplicates by i.e. filecontent, filename etc..

It then asks for a new path (dest dir), to which it will copy all files that are not "duplicates" and place them in a subdirectory by year of creation.
## How to use
Compile and run it, then write the path of the (src dir), then write "h" for list of commands.

Write "parameters" and follow instructions.

Then write "scan dir" and follow instructions.

Then write "find match in worklist" and wait for a while.

Then write "Find files to delete", it will not delete any files unless you later write "Delete duplicate files".

Then write "sort files" and write the path where you want the output of the program.
