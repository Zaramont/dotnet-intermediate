using System;

public class MessageSequencePart
{
    public MessageSequencePart()
    {

    }
    public MessageSequencePart(Guid id, string fileName, int positionInSequence, int sequenceSize, int positionInFile, byte[] data)
    {
        Id = id;
        FileName = fileName;
        PositionInSequence = positionInSequence;
        SequenceSize = sequenceSize;
        PositionInFile = positionInFile;
        Data = data;
    }

    public Guid Id { get; set; }
    public string FileName { get; set; }
    public int PositionInSequence { get; set; }
    public int SequenceSize { get; set; }
    public int PositionInFile { get; set; }
    public byte[] Data { get; set; }
}
