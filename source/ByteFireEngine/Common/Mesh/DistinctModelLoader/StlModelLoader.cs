using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ByteFireEngine.Common.Mesh.DistinctModelLoader;

public static class StlModelLoader
{
    public static Mesh LoadNonBinaryFile(string path)
    {

        var vertexData = new List<float>();
        Vector3 normal = new(0);

        foreach (var line in File.ReadAllLines(path))
        {
            var lineContent = line.Split(' ');
            
            if(lineContent.Length > 0)
                switch (lineContent.First())
                {
                    case "vertex":
                        vertexData.Add(float.Parse(lineContent[1], CultureInfo.InvariantCulture));
                        vertexData.Add(float.Parse(lineContent[3], CultureInfo.InvariantCulture));
                        vertexData.Add(float.Parse(lineContent[2], CultureInfo.InvariantCulture));
                        vertexData.Add(normal.X);
                        vertexData.Add(normal.Y);
                        vertexData.Add(normal.Z);
                        break;
                    case "facet":
                        normal = new Vector3(float.Parse(lineContent[2], CultureInfo.InvariantCulture), float.Parse(lineContent[3], CultureInfo.InvariantCulture), float.Parse(lineContent[4], CultureInfo.InvariantCulture));
                        break;
                    case "outer":
                    case "endloop":
                    case "endfacet":
                    case "solid":
                    case "endsolid":
                        break;
                    default: throw new ArgumentException(lineContent.First());
                }

        }

        var vertexAttributes = new VertexAttribute[]
        {
            new(3, VertexAttribPointerType.Float, 24, 0),
            new(3, VertexAttribPointerType.Float, 24, 12),
        };

        string[] lines = Array.ConvertAll(vertexData.ToArray(), n => n.ToString());
        File.WriteAllLines(@"C:\Users\Dennis\Desktop\testSTL1.txt", lines);
        Console.WriteLine("Array written to file successfully.");

        return new Mesh(vertexData.ToArray(), vertexAttributes);
    }
    public static Mesh LoadBinaryFile(string path)
    {
        var header = new byte[80];
        var triangleCountAsByte = new byte[4];

        using var fileStream = new FileStream(path, FileMode.Open);
        fileStream.Read(header, 0, 80);
        fileStream.Read(triangleCountAsByte, 0, 4);

        var triangleCount = BitConverter.ToInt32(triangleCountAsByte);

        var vertexDataList = new List<float>();

        for(int i = 0; i < triangleCount; i++)
        {
            var normalAsBytes = new byte[12];
            var vertex1AsBytes = new byte[12];
            var vertex2AsBytes = new byte[12];
            var vertex3AsBytes = new byte[12];
            var attributeAsBytes = new byte[2];

            fileStream.Read(normalAsBytes, 0, 12);
            fileStream.Read(vertex1AsBytes, 0, 12);
            fileStream.Read(vertex2AsBytes, 0, 12);
            fileStream.Read(vertex3AsBytes, 0, 12);
            fileStream.Read(attributeAsBytes, 0, 2);

            var normal = new Vector3(BitConverter.ToSingle(normalAsBytes, 0), BitConverter.ToSingle(normalAsBytes, 4), BitConverter.ToSingle(normalAsBytes, 8));

            var vertexData = new float[]
            {
                BitConverter.ToSingle(vertex1AsBytes, 0),
                BitConverter.ToSingle(vertex1AsBytes, 8),
                BitConverter.ToSingle(vertex1AsBytes, 4),
                normal.X,
                normal.Y,
                normal.Z,
                BitConverter.ToSingle(vertex2AsBytes, 0),
                BitConverter.ToSingle(vertex2AsBytes, 8),
                BitConverter.ToSingle(vertex2AsBytes, 4),
                normal.X,
                normal.Y,
                normal.Z,
                BitConverter.ToSingle(vertex3AsBytes, 0),
                BitConverter.ToSingle(vertex3AsBytes, 8),
                BitConverter.ToSingle(vertex3AsBytes, 4),
                normal.X,
                normal.Y,
                normal.Z,
            };
            
            vertexDataList.AddRange(vertexData);
        }
        
        var vertexAttributes = new VertexAttribute[]
        {
            new(3, VertexAttribPointerType.Float, 24, 0),
            new(3, VertexAttribPointerType.Float, 24, 12),
        };

        return new Mesh(vertexDataList.ToArray(), vertexAttributes);
    }
}