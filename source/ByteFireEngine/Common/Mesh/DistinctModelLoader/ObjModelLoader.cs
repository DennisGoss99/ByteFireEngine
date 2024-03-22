using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ByteFireEngine.Common.Mesh.DistinctModelLoader;

public static class ObjModelLoader
{
    internal static Mesh LoadObjFile(string path)
    {
        var linePos = 0;

        List<float> vertexData = new();
        List<float> vertexTextureData = new();
        List<float> vertexNormalData = new();

        List<string> faceData = new();

        string materialFilePath;

        foreach (var line in File.ReadLines(path))
        {
            var lineElements = line.Split(' ');

            if (lineElements.Length > 0)
            {
                switch (lineElements[0])
                {
                    case "v":
                        vertexData.Add(float.Parse(lineElements[1], CultureInfo.InvariantCulture));
                        vertexData.Add(float.Parse(lineElements[2], CultureInfo.InvariantCulture));
                        vertexData.Add(-float.Parse(lineElements[3], CultureInfo.InvariantCulture));
                        break;
                    case "vt":
                        vertexTextureData.Add(float.Parse(lineElements[1], CultureInfo.InvariantCulture));
                        vertexTextureData.Add(float.Parse(lineElements[2], CultureInfo.InvariantCulture));
                        break;
                    case "vn":
                        vertexNormalData.Add(float.Parse(lineElements[1], CultureInfo.InvariantCulture));
                        vertexNormalData.Add(float.Parse(lineElements[2], CultureInfo.InvariantCulture));
                        vertexNormalData.Add(-float.Parse(lineElements[3], CultureInfo.InvariantCulture));
                        break;
                    case "f":
                        faceData.Add(lineElements.Skip(1).Aggregate((it, acc) => it + " " + acc));
                        break;
                    case "mtllib":
                        materialFilePath = lineElements[1];
                        break;
                    case "o":
                    case "g":
                    case "l":
                    case "#":
                    case "usemtl":
                    case "s":
                        break;
                    default:
                        throw new ArgumentException($"Argument in line [{linePos}] is not supported '{lineElements[0]}'");
                }
            }
            linePos++;
        }

        Dictionary<(int, int?, int?), int> vertexMap = new Dictionary<(int, int?, int?), int>();
        List<float> dataArray = new List<float>();
        List<int> indexArray = new List<int>();

        foreach (var face in faceData)
        {
            var vertices = face.Split(' ');
            foreach (var vertex in vertices)
            {
                var parts = vertex.Split('/');

                int vertexIndex = int.Parse(parts[0]) - 1;
                int? textureIndex = parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]) ? int.Parse(parts[1]) - 1 : (int?)null;
                int? normalIndex = parts.Length > 2 && !string.IsNullOrWhiteSpace(parts[2]) ? int.Parse(parts[2]) - 1 : (int?)null;

                // Überprüfen, ob diese Kombination von Vertex-, Textur- und Normalen-Indizes bereits hinzugefügt wurde
                if (!vertexMap.TryGetValue((vertexIndex, textureIndex, normalIndex), out var globalIndex))
                {
                    // Vertex-Position hinzufügen
                    dataArray.Add(vertexData[vertexIndex * 3]);
                    dataArray.Add(vertexData[vertexIndex * 3 + 1]);
                    dataArray.Add(vertexData[vertexIndex * 3 + 2]);

                    // Normalen hinzufügen, falls vorhanden
                    if (normalIndex.HasValue && vertexNormalData.Count > 0)
                    {
                        dataArray.Add(vertexNormalData[normalIndex.Value * 3]);
                        dataArray.Add(vertexNormalData[normalIndex.Value * 3 + 1]);
                        dataArray.Add(vertexNormalData[normalIndex.Value * 3 + 2]);
                    }

                    // Texturkoordinaten hinzufügen, falls vorhanden
                    if (textureIndex.HasValue && vertexTextureData.Count > 0)
                    {
                        dataArray.Add(vertexTextureData[textureIndex.Value * 2]);
                        dataArray.Add(vertexTextureData[textureIndex.Value * 2 + 1]);
                    }

                    // Index des neuen Vertex im Map speichern und zum IndexArray hinzufügen
                    globalIndex = vertexMap.Count; // Jeder neue Vertex erhöht den globalen Index
                    vertexMap[(vertexIndex, textureIndex, normalIndex)] = globalIndex;
                }

                indexArray.Add(globalIndex);
            }
        }

        
        if(!faceData.First().Any(x => x == '/'))
        {
            Console.WriteLine("Vertex only");
            // Only vertex data
            return new Mesh(dataArray.ToArray(), indexArray.ToArray(), new VertexAttribute[]{
                new(3, VertexAttribPointerType.Float, 12, 0),
            });

        }else if(faceData.First().Split(' ')[0].Count(x => x == '/') == 1)
        {
            Console.WriteLine("Vertex, texture");
            // Vertex, texture
            return new Mesh(dataArray.ToArray(), indexArray.ToArray(), new VertexAttribute[]{
                new(3, VertexAttribPointerType.Float, 20, 0),
                new(2, VertexAttribPointerType.Float, 20, 12),
            });
        }else if(faceData.First().Split(' ')[0].Count(x => x == '/') == 2)
        {
            if(faceData.First().Split(' ')[0].Contains("//"))
            {
                Console.WriteLine("Vertex, normal 2");
                // Vertex, normal
                return new Mesh(dataArray.ToArray(), indexArray.ToArray(), new VertexAttribute[]{
                    new(3, VertexAttribPointerType.Float, 24, 0),
                    new(3, VertexAttribPointerType.Float, 24, 12),
                });
            }else{
                Console.WriteLine("Vertex, normal, texture");
                // Vertex, normal, texture
                return new Mesh(dataArray.ToArray(), indexArray.ToArray(), new VertexAttribute[]{
                    new(3, VertexAttribPointerType.Float, 32, 0),
                    new(3, VertexAttribPointerType.Float, 32, 12),
                    new(2, VertexAttribPointerType.Float, 32, 24),
                });
            }
        }else
            throw new ArgumentException("Face data is not supported");
    }
}