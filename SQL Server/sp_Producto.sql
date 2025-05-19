USE [PruebaTecnica]
GO
/****** Object:  StoredProcedure [dbo].[sp_Producto]    Script Date: 5/18/2025 4:02:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- SP para gestión de Productos
-- =============================================
ALTER   PROCEDURE [dbo].[sp_Producto]
(
    @ProductoID INT,
    @CodigoProducto VARCHAR(50),
    @NombreProducto VARCHAR(100),
    @Descripcion VARCHAR(255) = NULL,
    @UnidadMedidaID INT,
    @PrecioCompra DECIMAL(18,2),
    @PrecioVenta DECIMAL(18,2),
    @StockMinimo INT = 0,
    @StockMaximo INT = NULL,
    @Usuario VARCHAR(50),
	@Estado INT = 0,
	@Accion VARCHAR(50) -- 'INS' para Insert, 'UP' para Update, otro valor para Select
)
AS
BEGIN
    SET NOCOUNT ON;
   -- SET TRANSACTION ISOLATION LEVEL SNAPSHOT; -- Para evitar bloqueos en lecturas
    
    BEGIN TRY
        BEGIN TRANSACTION;
-- SELECT * FROM PRODUCTO
--SEL 
-- Exec sp_Producto @ProductoID=null, @CodigoProducto=null, @NombreProducto=null, @Descripcion='', @UnidadMedidaID=0,@PrecioCompra='0.80',@PrecioVenta='1.20', @StockMinimo='50', @StockMaximo='100',  @Usuario='irodriguez', @Estado=1,@Accion='SEL'
--UP  
-- Exec sp_Producto @ProductoID=6,	 @CodigoProducto='PROD-001', @NombreProducto='Arroz 1kg', @Descripcion='Arroz blanco grano largo chino', @UnidadMedidaID=2,@PrecioCompra='0.80',@PrecioVenta='1.20', @StockMinimo='50', @StockMaximo='100',  @Usuario='irodriguez', @Estado=1,@Accion='UP'
--INS 
-- Exec sp_Producto @ProductoID=null, @CodigoProducto='PROD-001', @NombreProducto='Arroz 1kg', @Descripcion='Arroz blanco grano largo', @UnidadMedidaID=2,@PrecioCompra='0.80',@PrecioVenta='1.20', @StockMinimo='50', @StockMaximo='100',  @Usuario='irodriguez', @Estado=1,@Accion='INS'
--ALTER DATABASE Israel SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;

		
		  IF @Accion = 'INS'
        BEGIN
                INSERT INTO Producto (CodigoProducto, NombreProducto, Descripcion, UnidadMedidaID, 
                              PrecioCompra, PrecioVenta, StockMinimo, StockMaximo, UsuarioCreacion,Activo,FechaCreacion)
        VALUES (@CodigoProducto, @NombreProducto, @Descripcion, @UnidadMedidaID, 
                @PrecioCompra, @PrecioVenta, @StockMinimo, @StockMaximo, @Usuario,1,GetDate());
            COMMIT TRANSACTION;
            SELECT 'OK' AS Resultado, 
                   'Registro creado exitosamente' AS Mensaje,
                   SCOPE_IDENTITY() AS UnidadMedidaID;

				   
        END
        -- UPDATE (Actualizar existente)
        ELSE IF @Accion = 'UP'
        BEGIN
              UPDATE Producto 
				SET 
					NombreProducto = @NombreProducto,
					Descripcion = @Descripcion,
					FechaModificacion = GETDATE(),
					UnidadMedidaID = @UnidadMedidaID,
					UsuarioModificacion = @Usuario
				WHERE Id = @ProductoID;

				
            
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
		  UPDATE Producto 
				SET 
					Activo = 0,
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @Usuario
				WHERE Id = @ProductoID;

				
            
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
             SELECT p.Id, p.CodigoProducto, p.NombreProducto, p.Descripcion, 
           um.Id as UnidadMedidaId, um.Nombre AS UnidadMedida,
           p.PrecioCompra, p.PrecioVenta, p.StockMinimo, p.StockMaximo,
           ISNULL(SUM(s.CantidadDisponible), 0) AS StockActual,
           p.Activo, p.FechaCreacion, p.UsuarioCreacion,
           p.FechaModificacion, p.UsuarioModificacion
    FROM Producto p
    JOIN UnidadMedida um ON p.UnidadMedidaID = um.Id
    LEFT JOIN Stock s ON p.ID = s.ProductoID
    WHERE (p.ID = @ProductoID OR @ProductoID IS NULL)
      AND (p.CodigoProducto LIKE '%' + @CodigoProducto + '%' OR @CodigoProducto IS NULL)
      AND (p.NombreProducto LIKE '%' + @NombreProducto + '%' OR @NombreProducto IS NULL)
      AND (p.Activo = 1 )
    GROUP BY p.ID, p.CodigoProducto, p.NombreProducto, p.Descripcion,
             um.Id, um.Nombre, p.PrecioCompra, p.PrecioVenta,
             p.StockMinimo, p.StockMaximo, p.Activo, p.FechaCreacion,
             p.UsuarioCreacion, p.FechaModificacion, p.UsuarioModificacion
    ORDER BY p.Id;


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
