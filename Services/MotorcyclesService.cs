using Microsoft.AspNetCore.Http.HttpResults;
using Moto.Controllers.DTO;
using Moto.DB;
using Moto.DB.Models.Products;
using Moto.Services.Interfaces;

namespace Moto.Services
{
    public class MotorcyclesService: IMotorcyclesService
    {
        private const string IMAGE_FILE_PREFIX = "motorcycles";
        private AppDbContext dbContext;
        private IFileService fileService;

        public MotorcyclesService(AppDbContext dbContext, IFileService fileService)
        {

            this.dbContext = dbContext;
            this.fileService = fileService;

            var motorcycles = dbContext.Motorcycles;
            if (motorcycles == null || motorcycles.Count() == 0)
            {
                var motors = new List<Motorcycle>
                {
                    new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                                        new Motorcycle
                    {
                        Name = "BMW F650GS",
                        Description = "Очень комфортен и удобен в любых сценариях использования, подойдет для начинающих. Низкий по седлу, легко рулится, при этом обладает впечатляющим запасом как по динамике, так и внедорожным качествам. Надежный и экономичный инжекторный мотор, изготовленный на заводе Rotax (Австрия), корректно работающий АБС. На мотоцикле уставлено примерно все, что можно придумать - защита рук, подогрев, высокое ветровое стекло, защитные дуги, защита картера, противоугонный блокиратор, полный комплект оригинальных кофров BMW (открываются родным ключом мотоцикла, боковые телескопические - два варианта объема).",
                        Category = Category.Motorcycle,
                        Cost = 410000,
                        Rating = 4.5f,
                        Brand = "BMW",
                        EnginePower = 50,
                        EngineCapacity = 652,
                    },
                    new Motorcycle
                    {
                        Name = "Nitro 2 - 250",
                        Description = "✔Moтoцикл nitrо 2 - 250 - дoступeн для покупки в нашем магазинe!",
                        Category = Category.Motorcycle,
                        Cost = 165000,
                        Rating = 3.5f,
                        Brand = "Nitro",
                        EnginePower = 18,
                        EngineCapacity = 250,
                    },
                    new Motorcycle
                    {
                        Name = "Suzuki Boulevard M109R, 2011",
                        Description = "Легенда, босс среди мотоциклов, не было и уже не будет более харизматичного мотоцикла в мире, не для всех, он не едет, а вырывает своим огромным катком куски планеты заставляя ее крутиться, его рык заставляет двигаться в потоке всех без исключения. Ускорение с места до скорости «я еле держусь за руль» заставит ужаснуться даже спортоводов. Мотоцикл не на учете, вы будете первым владельцем.",
                        Category = Category.Motorcycle,
                        Cost = 1100000,
                        Rating = 4f,
                        Brand = "Suzuki",
                        EnginePower = 125,
                        EngineCapacity = 1800,
                    },
                    new Motorcycle
                    {
                        Name = "Harley-Davidson Pan America, 2021",
                        Description = "Невероятный проект на базе Harley-Davidson Pan America\r\nЕдинственный такой.\r\nПолностью рабочая коляска , быстрый демонтаж. Мотоцикл можно использовать и без коляски.\r\nРекомендуем!",
                        Category = Category.Motorcycle,
                        Cost = 1990000,
                        Rating = 2f,
                        Brand = "Harley-Davidson",
                        EnginePower = 155,
                        EngineCapacity = 1250,
                    },
                };

                dbContext.Motorcycles.AddRange(motors);
                dbContext.SaveChanges();
            }
        }

        public async Task<IEnumerable<Motorcycle>> GetAll()
        {
            return dbContext.Motorcycles;
        }

        public async Task<Motorcycle> GetById(Guid id)
        {
            return await dbContext.Motorcycles.FindAsync(id);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            var motorcycle = await dbContext.Motorcycles.FindAsync(id);
            if (motorcycle is not null)
            {
                dbContext.Motorcycles.Remove(motorcycle);
                dbContext.SaveChanges();
                return true;
            }
            
            return false;
        }

        public async Task<IEnumerable<Motorcycle>> GetByPage(PageQueryParameters parameters)
        {
            var filtered = dbContext.Motorcycles.Where(m => m.Cost >= parameters.MinCost
                        && (parameters.MaxCost <= 0 || (parameters.MaxCost > 0 && m.Cost <= parameters.MaxCost))
                        && m.Rating >= parameters.MinRating && m.Rating <= parameters.MaxRating
                        && (parameters.NameQuery == null || (m.Name.Contains(parameters.NameQuery))));
            if(filtered.Count() == 0)
                return new List<Motorcycle>();
            return filtered
                .Skip((parameters.Page - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
        }

        public async Task<bool> UpdateById(Guid id, MotorcyclePostData changeData) 
        {
            var motorcycle = await dbContext.Motorcycles.FindAsync(id);
            if (motorcycle is not null)
            {
                if (changeData.Image != null)
                {
                    await fileService.DeleteFile(motorcycle.Name, IMAGE_FILE_PREFIX);
                    var imageFileName = await fileService.SaveFile(changeData.Image, IMAGE_FILE_PREFIX);
                    motorcycle.ImageFileName = $"{IMAGE_FILE_PREFIX}/{imageFileName}";
                }
                motorcycle.Name = changeData.Name;
                motorcycle.Description = changeData.Description;
                motorcycle.Rating = changeData.Rating;
                motorcycle.Cost = changeData.Price;
                motorcycle.EngineCapacity = changeData.EngineCapacity;
                motorcycle.EnginePower = changeData.EnginePower;
                motorcycle.Brand = changeData.Brand;
               
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Create(MotorcyclePostData createData)
        {
            if(createData.Image == null)
                return false;

            var imgFileName = await fileService.SaveFile(createData.Image, IMAGE_FILE_PREFIX);
            var newMotorcycle = new Motorcycle()
            {
                Name = createData.Name,
                Description = createData.Description,
                Category = "Motorcycle",
                Rating = createData.Rating,
                Brand = createData.Brand,
                Cost = createData.Price,
                EngineCapacity = createData.EngineCapacity,
                EnginePower = createData.EnginePower,
                ImageFileName = imgFileName,
            };

            await dbContext.AddAsync(newMotorcycle);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCount(PageQueryParameters parameters) {
            return dbContext.Motorcycles
                .Where(m => m.Cost >= parameters.MinCost
                        && (parameters.MaxCost <= 0 || parameters.MaxCost > 0 && m.Cost <= parameters.MaxCost)
                        && m.Rating >= parameters.MinRating && m.Rating <= parameters.MaxRating
                        && (parameters.NameQuery == null || (m.Name.Contains(parameters.NameQuery))))
                .Count();
        }
    }
}
