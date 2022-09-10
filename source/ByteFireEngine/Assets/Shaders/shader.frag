#version 330 core
out vec4 color;

in vec2 texCoord;
in vec3 normal;

uniform sampler2D texture0;

void main()
{
    //outputColor = texture(texture0, texCoord);

    color =vec4(vec3(1,1,1) * normal ,1) + vec4(1,1,1,1);
}