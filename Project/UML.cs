/*      "MiConexionLocal": "Server=(localdb)\\mssqllocaldb;Initial Catalog=BdD;Integrated Security=SSPI;"

        dotnet restore
        dotnet build OB-5.sln  
        cd WebMVC
        dotnet run --project WebMVC\WebMVC.csproj

        dotnet ef database update --project LogicaAccesoDatos --startup-project WebMVC

        http://localhost:5223/swagger/index.html


        azure dba password: NK'4!z'LU8-r>u~


    Empresa gestiona pagos realizados por sus equipos de trabajo:  
    --Cada USUARIO podra registrar los PAGOS qu realice mientras este vinculado a un EQUIPO en específico
    --Cada PAGO tiene un tipo de GASTO asociado (por ejemplo, nafta, comida, suscripcion a software, etc.)
    --Cada PAGO puede ser UNICO o RECURRENTE
    --Cada EQUIPO tiene una lista de USUARIOS (por ejemplo, Marketing, Desarrollo, Ventas)


Equipo -1---1o* Usuario -1---1o* Pago *---1 TipoGasto

Datos precargados: tipos de gasto disponible, metodos de pago a utilizar, usuarios administrador

    Equipo(-ID:int(autogenerado, unique), -NOMBRE:string(unique), -USUARIOS:List<Usuario>)       
    
    Usuario(-ID:int(autogenerado, unique), -NOMBRE:string, -APELLIDO:string, -CONTRASEÑA:string(min 8 caracteres), 
            -EMAIL:string(autogenerado, unique), -ROL:string)
            EMAIL:
            -El email se genera automáticamente combinando las primeras tres letras del nombre y las primeras tres letras del apellido y el dominio @laEmpresa.com
            -Si el nombre o el apellido tienen menos de tres letras, se utilizan completos. 
            -En caso de que el email generado ya exista, se le agrega un número al final para evitar duplicados. 
            -Los caracteres con tildes y otras alteraciones (eñes, vocales con tildes, diéresis, etc.) deberán remplazarse por sus versiones sin la alteración.
            -Por ejemplo, si el nombre fuera Juan Núñez, el correo generado debería ser juanun@laempresa.com y si ya existiera otro igual debería ser juanun1234@laempresa.com. El número (en el ejemplo 1234) deberá generarse en forma aleatoria. 

            Roles: Administrador, Gerente, Empleado

    Pago(-ID:int(autogenerado), -METODO:string(Credito o Efectivo), -GASTO:TipoGasto, -USUARIO:Usuario, -DESCRIPCION:string, -TIPOPAGO:string(Recurrente o Unico))
                    TIPOPAGO:
                    Recurrente: -Se repiten 1 vez al mes y es necesario saber las fechas desde y hasta del periodo en que se repiten
                                -Suscripciones, compras por cuotas
                                -Monto total se calcula multiplicando el monto del pago por la cantidad d emeses del periodo entre ambas fechas
                    Unico:  -Se pagan 1 sola vez, se registra la fecha de pago, los atributos comunes y ademas se registra tambien el numero de recibo de pago    
                            -Cargar nafta, pagar comida
                            -Monto total es el monto de pago      
    
 TipoGasto(-ID:int(autogenerado), -NOMBRE:string, -DESCRIPCION:string)   
                Estos gastos son generales y pueden reutilizarse en distintos pagos.
                Por ejemplo, el gasto "Auto" puede incluir tanto los gastos mensuales del vehículo como los relacionados con nafta y arreglos.
                Otro ejemplo es "Afters", que agrupa todos los gastos relacionados con salidas del equipo.  
                
              

                */  