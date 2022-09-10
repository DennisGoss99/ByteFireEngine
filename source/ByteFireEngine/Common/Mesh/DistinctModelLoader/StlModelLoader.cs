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
                        normal = new Vector3(float.Parse(lineContent[2], CultureInfo.InvariantCulture), float.Parse(lineContent[2], CultureInfo.InvariantCulture), float.Parse(lineContent[2], CultureInfo.InvariantCulture));
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

        return new Mesh(vertexData.ToArray(), vertexAttributes);
    }
    public static Mesh LoadBinaryFile(string path)
    {
        throw new NotImplementedException();
    }
}