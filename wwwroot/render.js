class RenderBackground {
    constructor() {
        this.renderLoopRunning = false;
    }
    initiateWebGl(fragCode, canvas) {
        //Prepare the canvas and get WebGL context 
        this.gl = canvas.getContext('webgl');
        //Define the geometry and store it in buffer objects 
        const vertices = [
            -1.0, 1.0, 0.0,
            -1.0, -1.0, 0.0,
            1.0, -1.0, 0.0,
            1.0, 1.0, 0.0
        ];
        this.indices = [3, 2, 1, 3, 1, 0];
        // Create a new buffer object
        const vertexBuffer = this.gl.createBuffer();
        // Bind an empty array buffer to it
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vertexBuffer);
        // Pass the vertices data to the buffer
        this.gl.bufferData(this.gl.ARRAY_BUFFER, new Float32Array(vertices), this.gl.STATIC_DRAW);
        // Unbind the buffer
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, null);
        // Create an empty buffer object to store Index buffer
        const indexBuffer = this.gl.createBuffer();
        // Bind appropriate array buffer to it
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, indexBuffer);
        // Pass the vertex data to the buffer
        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(this.indices), this.gl.STATIC_DRAW);
        // Unbind the buffer
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, null);
        //Create and compile Shader programs 
        // Vertex shader source code
        const vertCode = 'attribute vec3 coordinates;' +
            'void main(void) {' + ' gl_Position = vec4(coordinates, 1.0);' + '}';
        //Create a vertex shader object
        const vertShader = this.gl.createShader(this.gl.VERTEX_SHADER);
        //Attach vertex shader source code
        this.gl.shaderSource(vertShader, vertCode);
        //Compile the vertex shader
        this.gl.compileShader(vertShader);
        // Create fragment shader object
        const fragShader = this.gl.createShader(this.gl.FRAGMENT_SHADER);
        this.shaderTime = 0.0;
        // Attach fragment shader source code
        this.gl.shaderSource(fragShader, fragCode);
        // Compile the fragment shader
        this.gl.compileShader(fragShader);
        // Create a shader program object to store combined shader program
        const shaderProgram = this.gl.createProgram();
        // Attach a vertex shader
        this.gl.attachShader(shaderProgram, vertShader);
        // Attach a fragment shader
        this.gl.attachShader(shaderProgram, fragShader);
        // Link both programs
        this.gl.linkProgram(shaderProgram);
        // Use the combined shader program object
        this.gl.useProgram(shaderProgram);
        this.locationOfTime = this.gl.getUniformLocation(shaderProgram, "time");
        this.gl.uniform1f(this.locationOfTime, this.shaderTime);
        const locationOfResolution = this.gl.getUniformLocation(shaderProgram, "resolution");
        this.gl.uniform2f(locationOfResolution, canvas.width, canvas.height);
        this.locationOfMouse = this.gl.getUniformLocation(shaderProgram, "mouse");
        this.gl.uniform2f(this.locationOfMouse, this.mouseX, this.mouseY);
        /* Step 4: Associate the shader programs to buffer objects */
        //Bind vertex buffer object
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, vertexBuffer);
        // Bind index buffer object
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, indexBuffer);
        //Get the attribute location
        const coord = this.gl.getAttribLocation(shaderProgram, "coordinates");
        //point an attribute to the currently bound VBO
        this.gl.vertexAttribPointer(coord, 3, this.gl.FLOAT, false, 0, 0);
        //Enable the attribute
        this.gl.enableVertexAttribArray(coord);
        // Clear the canvas
        //gl.clearColor(0.0, 0.0, 0.0, 1.0);
        // Enable the depth test
        this.gl.enable(this.gl.DEPTH_TEST);
        // Clear the color buffer bit
        this.gl.clear(this.gl.COLOR_BUFFER_BIT);
        // Set the view port
        this.gl.viewport(0, 0, canvas.width, canvas.height);
    }
    renderLoop() {
        if (this.renderLoopRunning === false) {
            this.renderLoopRunning = true;
            this.renderLoop();
            return;
        }
        const animate = function (time, renderBackground) {
            // Draw the quad
            renderBackground.gl.drawElements(renderBackground.gl.TRIANGLES, renderBackground.indices.length, renderBackground.gl.UNSIGNED_SHORT, 0);
            renderBackground.shaderTime = renderBackground.shaderTime + 0.0025;
            renderBackground.gl.uniform1f(renderBackground.locationOfTime, renderBackground.shaderTime);
            renderBackground.gl.uniform2f(renderBackground.locationOfMouse, renderBackground.mouseX, renderBackground.mouseY);
            const now = new Date().getTime();
            renderBackground.frameCount++;
            renderBackground.elapsedTime += (now - renderBackground.lastTime);
            renderBackground.lastTime = now;
            if (renderBackground.elapsedTime >= 1000) {
                renderBackground.fps = renderBackground.frameCount;
                renderBackground.frameCount = 0;
                renderBackground.elapsedTime = 0;
                //document.getElementById('footer').innerHTML = fps + ' Frames Per Second';
            }
            window.requestAnimationFrame(function () {
                animate(time, renderBackground);
            });
        };
        animate(0, this);
    }
}
const renderBackground = new RenderBackground();
const webGLCanvas = document.getElementById('webGLCanvasIdentifier');
// As with JSON, use the Fetch API & ES6
fetch('shader.txt')
    .then(response => response.text())
    .then(data => {
    console.log(data);
    webGLCanvas.width = window.innerWidth;
    webGLCanvas.height = window.innerHeight;
    renderBackground.initiateWebGl(data, webGLCanvas);
    renderBackground.renderLoop();
});
//# sourceMappingURL=render.js.map