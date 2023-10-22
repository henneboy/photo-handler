namespace photo_handler ;

public enum Criteria
{
    filename,
    filesize,
    lastmodified,
    created,
    filecontent,
    filetype
}
public delegate bool Comparison(FileInfo f1, FileInfo f2);
