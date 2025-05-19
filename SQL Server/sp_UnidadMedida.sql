USE [PruebaTecnica]
GO
/****** Object:  StoredProcedure [dbo].[sp_UnidadMedida]    Script Date: 5/18/2025 4:37:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
* AUTOR: Israel Rodriguez
* FECHA: 
* DESCRIPCIÓN: Procedimientos para gestión de inventario con control de transacciones
*              y minimización de bloqueos.
******************************************************************************/

-- =============================================
-- SP para gestión de Unidades de Medida
-- =============================================
ALTER   PROCEDURE [dbo].[sp_UnidadMedida]
(
	@UnidadMedidaID INT,
    @Codigo VARCHAR(10),
    @Nombre VARCHAR(50),
    @Descripcion VARCHAR(100) = NULL,
	@Accion VARCHAR(50) -- 'INS' para Insert, 'UP' para Update, otro valor para Select
)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ COMMITTED; -- Evita bloqueos innecesarios
   --SEL Exec sp_UnidadMedida @UnidadMedidaID=null, @Codigo=null, @Nombre=null, @Descripcion='Peso en Tonelada',  @Accion='SEL'
   --UP  Exec sp_UnidadMedida @UnidadMedidaID=4, @Codigo=null, @Nombre='Segundo', @Descripcion='seg', @Accion='UP'
   --INS Exec sp_UnidadMedida @UnidadMedidaID=null, @Codigo='s', @Nombre='Segundo', @Descripcion='Seg', @Accion='INS'
   
   -- Select Exec sp_UnidadMedida_Insertar @UnidadMedidaID=null, @Codigo='TK', @Nombre=null, @Descripcion='Peso en Tonelada', @Usuario='irodriguez', @Accion='SEL'
  
   
   
   -- Select * from UnidadMedida
	/*INSERT INTO UnidadesMedida (Codigo, Nombre, Descripcion)
	VALUES ('UN', 'Unidad', 'Elemento individual'),
	('KG', 'Kilogramo', 'Peso en kilogramos'),
	('LT', 'Litro', 'Volumen en litros'),
	('M', 'Metro', 'Longitud en metros');
	*/
    BEGIN TRY
        BEGIN TRANSACTION;
        
       

		


		  IF @Accion = 'INS'
        BEGIN
            INSERT INTO UnidadMedida (Codigo, Nombre, Descripcion)
        VALUES (@Codigo, @Nombre, @Descripcion);
            
			 COMMIT TRANSACTION;

            SELECT 'OK' AS Resultado, 
                   'Registro creado exitosamente' AS Mensaje,
                   SCOPE_IDENTITY() AS UnidadMedidaID;
        END
        -- UPDATE (Actualizar existente)
        ELSE IF @Accion = 'UP'
        BEGIN
	
              UPDATE UnidadMedida 
				SET 
					Nombre = @Nombre,
					Descripcion = @Descripcion
				WHERE Id = @UnidadMedidaID;
            
			IF @@ROWCOUNT = 0
			BEGIN
				ROLLBACK TRANSACTION;
				SELECT 'Error' AS Resultado, 
					   'No se encontró el registro con el ID especificado' AS Mensaje;
			END
			ELSE
			BEGIN
				COMMIT TRANSACTION;
				SELECT 'OK' AS Resultado, 
					   'Registro actualizado exitosamente' AS Mensaje;
			END
        END
        -- SELECT (Consulta por defecto)
        ELSE
        BEGIN

		 COMMIT TRANSACTION;

            SELECT 
                Id,
                Codigo,
                Nombre,
                Descripcion
            FROM UnidadMedida
            WHERE 
                (@UnidadMedidaID IS NULL OR Id = @UnidadMedidaID)
                
            ORDER BY Id;
        END

        
       
      
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        SELECT 'ERROR' AS Resultado, 
               ERROR_MESSAGE() AS Mensaje,
               ERROR_NUMBER() AS CodigoError;
    END CATCH
END;