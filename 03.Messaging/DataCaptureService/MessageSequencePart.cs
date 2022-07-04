using System;

public class MessageSequencePart
{
	public MessageSequencePart(Guid id, string fileName, int positionInSequence, int sequenceSize, int positionInFile, byte[] data)
	{
        Id = id;
        FileName = fileName;
        PositionInSequence = positionInSequence;
        SequenceSize = sequenceSize;
        PositionInFile = positionInFile;
        Data = data;
    }

    public Guid Id { get; }
    public string FileName { get; }
    public int PositionInSequence { get; }
    public int SequenceSize { get; }
    public int PositionInFile { get; }
    public byte[] Data { get; }
}
