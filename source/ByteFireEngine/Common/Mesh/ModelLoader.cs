using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteFireEngine.Common.Mesh.DistinctModelLoader;
using OpenTK.Graphics.ES20;

namespace ByteFireEngine.Common.Mesh;

public static class ModelLoader
{
    public static Mesh LoadObjFile(string path)
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
                        vertexData.Add(float.Parse(lineElements[3], CultureInfo.InvariantCulture));
                        break;
                    case "vt":
                        vertexTextureData.Add(float.Parse(lineElements[1], CultureInfo.InvariantCulture));
                        vertexTextureData.Add(float.Parse(lineElements[2], CultureInfo.InvariantCulture));
                        break;
                    case "vn":
                        vertexNormalData.Add(float.Parse(lineElements[1], CultureInfo.InvariantCulture));
                        vertexNormalData.Add(float.Parse(lineElements[2], CultureInfo.InvariantCulture));
                        vertexNormalData.Add(float.Parse(lineElements[3], CultureInfo.InvariantCulture));
                        break;
                    case "f":
                        faceData.Add(lineElements.Skip(1).Aggregate((it, acc) => acc + " " + it));
                        break;
                    case "o":
                        break;
                    case "g":
                        break;
                    case "l":

                        break;
                    case "#":
                        break;
                    case "mtllib":
                        materialFilePath = lineElements[1];
                        break;
                    case "usemtl":
                        break;
                    case "s":
                        break;
                    default:
                        throw new ArgumentException($"Argument in line [{linePos}] is not supported '{lineElements[0]}'");
                }
            }
            linePos++;
        }


        var slashCount = faceData.First().Split(' ')[0].Count(it => it == '/');

        switch (slashCount)
        {
            case 0:

                foreach (var s in faceData)
                {
                    var facePoints = s.Split(' ');
                }

                break;
            case 2:




                break;
        }

        foreach (var s in faceData)
        {
            var facePoints = s.Split(' ');

                

        }

        return new Mesh(new float[] { }, new VertexAttribute[] { });
    }

    public static Mesh LoadStlFile(string path)
    {
        char[] buffer = new char[5];

        using StreamReader reader = new StreamReader(path);
        reader.Read(buffer, 0, 5);

        if ("solid".Equals(new string(buffer), StringComparison.Ordinal))
            return StlModelLoader.LoadNonBinaryFile(path);

        return StlModelLoader.LoadBinaryFile(path);
    }

}
