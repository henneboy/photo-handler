namespace photo_handler ;

public enum Criteria
{
    filename,
    filesize,
    lastmodified,
    creationTime,
    filecontent,
    filetype
}
public delegate bool Comparison(FileInfo f1, FileInfo f2);
