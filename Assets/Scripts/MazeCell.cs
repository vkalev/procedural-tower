public class MazeCell
{
    public Tile Tile { get; set; } 
    public int X { get; set; }
    public int Z { get; set; }
    public bool LeftWall { get; set; } 
    public bool RightWall { get; set; }
    public bool BottomWall { get; set; }
    public bool TopWall { get; set; }

    public MazeCell(int x, int z, Tile tile)
    {
        this.Tile = tile;
        this.X = x;
        this.Z = z;
        this.LeftWall = true;
        this.RightWall = true;
        this.BottomWall = true;
        this.TopWall = true;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as MazeCell);
    }

    public bool Equals(MazeCell other)
    {
        return other != null &&
            this.X == other.X &&
            this.Z == other.Z;
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
