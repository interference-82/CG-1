var camera, controls, scene, renderer, geometry;
var properties = {
    size: 50, // размер объекта
    a: 1, b: 1.2, c: 1.5, // параметры эллипсоида
    approximation: 15, // количество полигонов
    phi: 2*Math.PI, // 2pi - угол для x
    theta: Math.PI, // pi - для z
    color: 'Green', // цвет объекта
    reflectivity: 1.0, // светоотражающее свойство
    moveX: 0, moveY: 0, moveZ: 0, // смещение по осям x, y, z
    x: 0, y:0, z:0, // тут храним текущее положение объекта
    speed:0
};
var time=0;
var newtime =0;
//запускаем при загрузке окна
window.onload = function() {
    //если WebGL не доступен - сообщение об ошибке
    if ( WEBGL.isWebGLAvailable() === false ) {
        document.body.appendChild( WEBGL.getWebGLErrorMessage() );
    }
    //инициализация
    init();
    //запускаем рекурсивную функцию анимации, отрисовывающую каждый кадр
    animate();
}

function init() {
    //Тут создаем GUI
    var gui = new dat.GUI();
    //gui.add(properties, 'size').min(10).max(100).step(1);

    gui.add(properties, 'approximation').min(4).max(100).step(1);
    gui.add( properties, 'reflectivity', 0.1, 2 );
    gui.add(properties, 'speed',{ Stopped: 0, Slow: 600, Fast: 300 });
    
    //сцена
    scene = new THREE.Scene();
    //цвет сцены
    scene.background = new THREE.Color( 0x191919 );
    //туман для красоты
    //scene.fog = new THREE.FogExp2( 0xcccccc, 0.002 );
    //renderer
    renderer = new THREE.WebGLRenderer( { antialias: true } );
    renderer.setPixelRatio( window.devicePixelRatio );
    renderer.setSize( window.innerWidth, window.innerHeight );
    document.getElementById('WebGL-output').setAttribute('width', window.innerWidth);
    document.getElementById('WebGL-output').setAttribute('height', window.innerHeight);
    document.getElementById('WebGL-output').appendChild(renderer.domElement); 
    //камера
    camera = new THREE.PerspectiveCamera( 60, window.innerWidth / window.innerHeight, 1, 1000 );
    camera.position.set( 400, 200, 0 );
    // controls (позволяет управлять камерой при помощи мыши и клавиатуры)
    controls = new THREE.OrbitControls( camera, renderer.domElement );
    controls.enableDamping = true;
    controls.dampingFactor = 0.25;
    controls.screenSpacePanning = false;
    controls.minDistance = 100;
    controls.maxDistance = 500;
    controls.maxPolarAngle = Math.PI / 2;
    //обработка события "поднятие клавиши"
    document.addEventListener("keyup", keyUpTextField, false);
    
    // освещение
    //точечное освещение
    var light = new THREE.DirectionalLight( 0xffffff );
    light.position.set( 1, 1, 1 );
    scene.add( light );
    //var light = new THREE.DirectionalLight( 0x002288 );
    //light.position.set( - 1, - 1, - 1 );
    //scene.add( light );
    //общий свет, светит "отовсюду"
    var light = new THREE.AmbientLight( 0xffbdd3 );
    scene.add( light );
    //обработка события "изменение размера окна"
    window.addEventListener( 'resize', onWindowResize, false );
}

//если была нажата клавиша r - сброс положения камеры и объекта
function keyUpTextField(e) {
    var keyCode = e.keyCode;
        if(keyCode==82) {
            controls.reset();
            properties.x = properties.y = properties.z = 0;
        }
}

//если изменился размер окна - должен измениться размер renderer'а и настройки камеры
function onWindowResize() {
    //настраиваем пропорции камеры во избежание искажений
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    //настраиваем окно renderer'а для того, чтобы оно занимало весь экран
    renderer.setSize( window.innerWidth, window.innerHeight );
}

//функция, вызывающаяся раз за разом, в которой и происходит отрисовка объекта
function animate() {
    requestAnimationFrame( function() {animate();} );
    time++;
    controls.update();
    
    //создание геометрии объекта
    geometry = new THREE.SphereGeometry( properties.size, properties.approximation, properties.approximation, 0, properties.phi, 0, properties.theta);
    geometry.applyMatrix( new THREE.Matrix4().makeScale( properties.a, properties.b, properties.c ) );
    //geometry.applyMatrix( new THREE.Matrix4().makeScale( Math.cos(time/100) , Math.sin(time/100), 1) );
    if(properties.speed!=0){
        for (let i=0; i<geometry.vertices.length; ++i){
            geometry.vertices[i].x *= Math.cos(time/properties.speed);
            geometry.vertices[i].y*=Math.sin(time/properties.speed+geometry.vertices[i].x);
        }
        //newtime=time/properties.speed;
    }
    var material = new THREE.MeshPhysicalMaterial({color:  new THREE.Color( 0xc30a0d4 ), flatShading: true, side: THREE.DoubleSide, reflectivity: properties.reflectivity});
    //удаление предыдущего объекта со сцены
    var selectedObject = scene.getObjectByName("mesh");
    scene.remove( selectedObject );
    //создание нового меша, расположение его на сцене
    var mesh = new THREE.Mesh( geometry, material);
    properties.x += properties.moveX;
    properties.y += properties.moveY;
    properties.z += properties.moveZ;

    //6 ЛД

    mesh.position.x = properties.x;
    mesh.position.y = properties.y;
    mesh.position.z = properties.z;
    mesh.name = "mesh";
    scene.add(mesh);
    //рендерим сцену
    render();
}

function render() {
    renderer.render( scene, camera );
}
