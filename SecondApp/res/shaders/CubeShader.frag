#version 330 core
out vec4 FragColor;

in vec2 TexCoord;
in vec3 FragPos;
in vec3 Normal;

uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightColor;

// texture samplers
uniform sampler2D texture1;
uniform sampler2D texture2;

vec3 ambientLight;

void main()
{
	float ambientStrength = 0.05;
	ambientLight = lightColor * ambientStrength;

	vec3 norm = normalize(Normal);
	vec3 lightDir = normalize(lightPos - FragPos);
	float diff = max(dot(norm, lightDir), 0.0f);
	vec3 diffuse = diff * lightColor;

	float specularStrength = 0.5;
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 reflectDir = reflect(-lightDir, norm);  
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	vec3 specular = specularStrength * spec * lightColor;

	FragColor = mix(texture(texture1, TexCoord), texture(texture2, TexCoord), 0.1) * vec4((ambientLight + diffuse + specular), 1.0f);
}