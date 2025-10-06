tenemos que reconfigurar y re modelar como se va a hacer el flujo de registro e inicio de sesion de usuarios, no usaremos auth0 hay que borrar
TODO rastro de auth0 y su incorporacion debido a que dio errores tecnicos, usaremos JWT y asp.NET con identity para la creacion de 
usuarios, inicio de sesion y asignacion de roles.
fijate en requeriments esta la explicacion, incorpora la nueva forma de registro e inicio de sesion. usando arquitectura
limpia y solid, deben ser 2 endpoints 1 para que los usuarios se registren y obtengan el rol Medico en su usuario ni bien
se registren y otro para que se registren y obtengan el rol Paciente, ambos endpoints, en ambos casos del registro se debe configurar
para que se mande un correo de confirmacion con un codigo de 6 digitos, y el usuario debe ingresar ese codigo en otro endpoint enviando el correo
y el codigo pero primero empecemos refactorizando y haciendo el registro y el inicio de sesion, luego hacemos lo del codigo de confirmacion.
elimina todo de auth0.
haz que en program o donde corresponda al iniciar la app automaticamente se creen los roles en las tablas.
si necesitas paquetes ponlos en este md separa con una linea o algo y yo los instalare.
