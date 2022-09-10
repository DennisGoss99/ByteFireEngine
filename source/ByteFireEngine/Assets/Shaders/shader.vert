#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 normal;
out vec3 fragPos;
out vec2 texCoord;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    fragPos = vec3(vec4(aPos, 1.0) * model);
    normal = aNormal * mat3(transpose(inverse(model)));
    texCoord = aTexCoords;
}