namespace photo_handler;

public interface ISortingService
{
	public SortingResults PerformSorting(bool allowDuplicates, State state);
}

public record SortingResults(long FromByteSize, long ToByteSize, long? NumberOfDuplicateFiles, TimeSpan ExecutionTime);
