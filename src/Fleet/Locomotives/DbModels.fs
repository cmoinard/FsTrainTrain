module Fleet.Locomotives.DbModels

type DbLocomotive =
    {
        Id: int
        Brand: string
        Model: string
        WeightInTons: decimal
        MaxTractionInTons: decimal
    }
    
type DbNewLocomotive =
    {
        Brand: string
        Model: string
        WeightInTons: decimal
        MaxTractionInTons: decimal
    }