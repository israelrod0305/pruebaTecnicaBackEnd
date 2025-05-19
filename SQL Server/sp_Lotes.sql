USE [PruebaTecnica]
GO
/****** Object:  StoredProcedure [dbo].[sp_Lote]    Script Date: 5/18/2025 4:13:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- SP para gestión de Lotes
-- =============================================
ALTER   PROCEDURE [dbo].[sp_Lote]
(
	@LoteID INT,
    @ProductoID INT,
    @NumeroLote VARCHAR(50),
    @FechaFabricacion DATE = NULL,
    @FechaVencimiento DATE = NULL,
    @CantidadInicial INT,
    @Usuario VARCHAR(50),
	@Estado VARCHAR(50),
	@Accion VARCHAR(50) 
)
AS
BEGIN
    SET NOCOUNT ON;
    --SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

	--select * from Lote
	--Truncate table Lotes
   --SEL Exec sp_Lote @LoteID=null, @ProductoID=null, @NumeroLote=null, @FechaFabricacion='2023-01-15', @FechaVencimiento='2024-01-15',@CantidadInicial=100,  @Usuario='irodriguez',@Estado='Activo' ,@Accion='SEL'
   --UP  Exec sp_Lote @LoteID=5,    @ProductoID=6, @NumeroLote='LOTE-2024-001', @FechaFabricacion='2023-01-15', @FechaVencimiento='2023-01-30',@CantidadInicial=100, @Usuario=null, @Estado='Activo',@Accion='UP'
   --INS Exec sp_Lote @LoteID=null, @ProductoID=6, @NumeroLote='LOTE-2029-008', @FechaFabricacion='2023-01-15', @FechaVencimiento='2023-01-15',@CantidadInicial=100,  @Usuario='irodriguez', @Estado='Activo',@Accion='INS'
  
   -- (1, 'LOTE-2023-001', '2023-01-15', '2024-01-15', 100, 100),
   
    BEGIN TRY
        BEGIN TRANSACTION;

	  IF @Accion = 'INS'
        BEGIN

			INSERT INTO Lote (ProductoID, NumeroLote, FechaFabricacion, FechaVencimiento, 
							CantidadInicial, CantidadDisponible, UsuarioCreacion,FechaRegistro,Estado)
			VALUES (@ProductoID, @NumeroLote, @FechaFabricacion, @FechaVencimiento,
				@CantidadInicial, @CantidadInicial, @Usuario, GETDATE(),1);                
			-- Actualizar stock
			INSERT INTO Stock (ProductoID, LoteID, CantidadDisponible, FechaActualizacion)
			VALUES (@ProductoID, SCOPE_IDENTITY(), @CantidadInicial, GETDATE());

			COMMIT TRANSACTION;
            
            SELECT 'OK' AS Resultado, 
                   'Registro creado exitosamente' AS Mensaje,
                   SCOPE_IDENTITY() AS Id;
			
	
        END
        -- UPDATE (Actualizar existente)
        ELSE IF @Accion = 'UP'
        BEGIN
              UPDATE Lote
				SET 
					NumeroLote = @NumeroLote,
					FechaFabricacion = @FechaFabricacion,
					FechaVencimiento = @FechaVencimiento,
				--	Descripcion = @Descripcion,
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @Usuario
				WHERE Id = @LoteID;
            COMMIT TRANSACTION;
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
		 ELSE IF @Accion = 'DEL'
        BEGIN
              UPDATE Lote
				SET 
					Estado = '0',
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @Usuario
				WHERE Id = @LoteID;
           
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
				SELECT l.Id,p.Id as ProductoId, p.NombreProducto, l.NumeroLote, 
				l.FechaFabricacion, l.FechaVencimiento,
				l.CantidadInicial, l.CantidadDisponible, l.Estado
				FROM Lote l
				JOIN Producto p ON l.ProductoID = p.Id
				WHERE 
				-- l.Estado = @Estado AND p.Estado = 1
				 (l.Id = @LoteID OR @LoteID IS NULL)
				AND (l.NumeroLote LIKE '%' + @NumeroLote + '%' OR @NumeroLote IS NULL)
				AND (l.Estado = '1')

				ORDER BY l.FechaVencimiento;
				  

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
