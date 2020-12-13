namespace Fleet.Migrations

open SimpleMigrations

[<Migration(1L, "Create Locomotive schema")>]
type Initial() =
    inherit Migration()

    override this.Up() =
        this.Execute
            @"CREATE TABLE locomotives (
                id serial PRIMARY KEY,
                brand VARCHAR(50) NOT NULL,
                model VARCHAR(50) NOT NULL,
                weight_in_tons NUMERIC(6,3) NOT NULL,
                max_traction_in_tons NUMERIC(9,3) NOT NULL
            )"
            
        this.Execute(
            @"INSERT INTO locomotives(brand, model, weight_in_tons, max_traction_in_tons)
            VALUES ('SuperMotive', 'Loco X1', 100, 2000)")
    
    override this.Down() =
        this.Execute "DROP TABLE locomotives"
    
    

