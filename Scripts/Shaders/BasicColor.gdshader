shader_type canvas_item;
uniform vec4 color : source_color = vec4(1);

void fragment() {
	// Called for every pixel the material is visible on.
  COLOR = color;
}
