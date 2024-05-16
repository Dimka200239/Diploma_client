using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.ML;
using System.IO;
using System;

namespace client.Model
{
    [Table("MachineLearningModel")]
    public class MachineLearningModel
    {
        [Key]
        public int Id { get; set; }

        public string ModelData { get; set; }

        public DateTime CreatedDate { get; set; }
        public int CountOfData { get; set; }

        public MachineLearningModel CreateFromModel(ITransformer model, IDataView schema, int countOfData)
        {
            using (var memoryStream = new MemoryStream())
            {
                var mlContext = new MLContext();
                mlContext.Model.Save(model, schema.Schema, memoryStream);
                return new MachineLearningModel
                {
                    ModelData = Convert.ToBase64String(memoryStream.ToArray()),
                    CreatedDate = DateTime.UtcNow,
                    CountOfData = countOfData
                };
            }
        }

        public ITransformer DeserializeModel(MLContext mlContext, out DataViewSchema schema)
        {
            var modelBytes = Convert.FromBase64String(ModelData);
            using (var memoryStream = new MemoryStream(modelBytes))
            {
                return mlContext.Model.Load(memoryStream, out schema);
            }
        }
    }
}
