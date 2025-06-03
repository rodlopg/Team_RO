# DESARROLLO DE PLUGINS - FINAL PROJECT documentacion

# Trabajo hecho por:

Jorge Enrique Ruiz Liera

# El documento contiene las actividades que realice y el nombre de los scripts que implemente. Para mas detalles todo esta dividido de acuerdo a la actividad de realizada. Es decir, en la seccion 1 "Agregar efectos de particulas" se encuentra el nombre de los scripts para las particulas + explicacion, tambien se incluye comentarios y/o observaciones.

- Room Design -> Agregar efectos de particulas
- Design -> Realizar secuencia inicial
- Design -> Agregar efectos visuales al enemigo
- Design -> Agregar efectos sonoros al enemigo
- Design -> Realizar secuencia final

# Seccion 1: Agregar efectos de particulas

## Scripts

- **`ParticleElectric.cs`** - Crea un efecto de particulas electricas al colisionar con un objeto.
  Variables principales:

  - particlePrefab: Prefab del sistema de particulas a instanciar.
  - effectTime: Duracion del efecto en segundos.

  Funcionalidad: Instancia particulas cuando un objeto con el tag especificado Player entra en contacto y las destruye despues de un tiempo.

- **`ParticleSmoke.cs`** - Activa/desactiva particulas de humo basado en la distancia a un objeto.
  Variables principales:

  - distanciaActivacion: Distancia para activar las particulas.
  - usarTriggerCollider: Alternar entre usar collider trigger o distancia.

  Funcionalidad: Muestra particulas cuando un objeto se acerca y las oculta cuando se aleja.

- **`SimpleSparkEmitter.cs`** - Emite chispas de particulas a intervalos regulares.
  Variables principales:

  - cadaCuantosSegundos: Intervalo entre emisiones.
  - usarTriggerCollider: Alternar entre usar collider trigger o distancia.

  Funcionalidad: Emite particulas en intervalos configurables con cantidad controlable.

- **`WaterDropSystem.cs`** - Sistema de goteo de agua con particulas y sonido.
  Variables principales:

  - intervaloMinimo/Maximo: Rango de tiempo entre gotas.
  - sonidosGota: Sonidos aleatorios para las gotas.

  Funcionalidad: Emite gotas de agua con particulas en intervalos aleatorios, con control manual y automatico.

## Comentario: Existe una escena llamada "Particles Simulation" en la carpeta Scenes, ahi estan puestos los objetos con su script correspondiente. Los objetos los puse de ejemplo para hacer experimentacion, testear el comportamiento y hacer ajustes.

## Observacion: Los scripts se deben anadir al objeto que va generar el efecto o particulas. Ejemplo, al objeto de trampa de electricidad se le anade el script "Particle Electric". Es posible hacer ajustes en el Inspector para modificar su comportamiento.

# Seccion 2: Realizar secuencia inicial

    La secuencia inicial es una cinematica en formato .mp4 con el nombre "SecuenciaInicial". No es asset, ni animacion. Sin embargo para la realizacion de esta, lo que hice para esta actividad, fue crear una escena llamada SecuenciaInicial y apartir de ella hice el video, luego lo edite en una aplicacion externa. La escena si incluye un pequeno script, pero su uso es meramente estetico.

## Scripts

- **`BlinkingLight.cs`** - Controla el parpadeo de una luz en Unity.
  Variables principales:

  - tiempoEncendida: Tiempo que la luz permanece encendida (en segundos).
  - tiempoApagada: Tiempo que la luz permanece apagada (en segundos).
  - iniciarAutomaticamente: Si el parpadeo comienza automaticamente al iniciar.

  Funcionalidad: Alterna el estado de un componente Light entre encendido y apagado segun los tiempos configurados.

## Comentario: La escena Secuencia Inicial no tiene uso, pues no se incluira como parte de la secuencia de escenas. Aun asi, se podria reutilizar en el futuro.

# Seccion 3: Agregar efectos visuales al enemigo

No se han utilizado scripts para este assignment.
Para esta actividad agregue el package "Visual Effect Graph", sirve para trabajar con efectos visuales. En la carpeta TeamRO_DIR hay un Visual Effect Asset. El asset es util para gestion y personalizacion de efectos visuales. El nombre del asset que servira para el enemigo principal es "vfx_MainEnemy".

## Assets

### Vfx_MainEnemy

    Modo de uso: Para que funcione correctamente, hay que colocar el asset en la hierarchy. En el Inspector, en la parte de Properties esta un atributo llamado Skinned Mesh Renderer 1, ese debe estar encendido. Al atributo hay que colocarle el mesh del enemigo "skeletonZombie", el cual es el enemigo principal.

#### Required Components

VFX property Binder - Es importante que el asset incluya este componente, que resulta ser un script. En property Bindings, asegurarse de incluir la propiedad Transform y anadirle en Target el esqueleto del enemigo, el nombre es mixamorig:Hips.

## Comentario: Existen dos assets mas de visual effects, llamados "Vfx_enemies_test_1" y "Vfx_test_3". Estos unicamente son tests, ya no tienen uso.

## Observacion: En caso de no aplicar correctamente el mesh del enemigo o no anadir el esqueleto del enemigo en donde se indico anteriormente, los efectos visuales van a permanecer en el lugar donde spawnee el enemigo principal.

# Seccion 4: Agregar efectos sonoros al enemigo

- **`EnemyAudioEffect.cs`** - Reproduce efectos de audio cuando un jugador se acerca a un enemigo.
  Variables principales:

  - detectionRange: Rango de deteccion del jugador.
  - audioClips: Array de clips de audio a reproducir.
  - Funcionalidad: Detecta jugadores en un rango especifico y reproduce sonidos aleatorios cuando estan cerca.

## Comentario: En caso de que no se escuche o no se perciba el audio, hay que anadir el script "EnemyAudioEffect" a el prefab o los prefabs de los enemigos que se van a instanciar/spawnear.

## Comentario: Los archivos de audio se encuentran en la carpeta Resources, subcarpeta Audio.

## Observacion: En el Inspector, en Audio Source no debe estar activado el cuadrito Loop.

# Seccion 5: Realizar Secuencia Final

La secuencia final es una cinematica en formato .mp4 con el nombre "SecuenciaFinal". No es asset, ni animacion. Sin embargo para la realizacion de esta, lo que hice para esta actividad, fue crear una escena llamada SecuenciaFinal y apartir de ella hice el video, luego lo edite en una aplicacion externa.
