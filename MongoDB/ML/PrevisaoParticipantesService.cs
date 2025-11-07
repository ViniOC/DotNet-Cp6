using EventosApi.ML; // Para nossos modelos de dados (Input/Output)
using Microsoft.ML;
using System;
using System.Collections.Generic;

namespace EventosApi.ML
{
    public class PrevisaoParticipantesService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public PrevisaoParticipantesService()
        {
            _mlContext = new MLContext(seed: 0);
        }

        public void TrainModel()
        {
            var trainingData = GetFakeTrainingData();
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // 3. DEFINIR O PIPELINE DE TREINAMENTO (COM A CORREÇÃO FINAL)
            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey(
                    outputColumnName: "CategoriaKey",
                    inputColumnName: nameof(EventoPrevisaoInput.Categoria))
                .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
                    outputColumnName: "CategoriaEncoded",
                    inputColumnName: "CategoriaKey"))

                // --- MUDANÇA ESTÁ AQUI ---
                // 4. NORMALIZAR os números para que fiquem na mesma escala (0 a 1)
                .Append(_mlContext.Transforms.NormalizeMinMax(
                    outputColumnName: "MesNormalizado",
                    inputColumnName: nameof(EventoPrevisaoInput.Mes)))
                .Append(_mlContext.Transforms.NormalizeMinMax(
                    outputColumnName: "CapacidadeNormalizada",
                    inputColumnName: nameof(EventoPrevisaoInput.CapacidadeMaxima)))

                // 5. CONCATENAR as features DEPOIS de normalizadas
                .Append(_mlContext.Transforms.Concatenate("Features",
                    "MesNormalizado",             // Usamos a coluna normalizada
                    "CapacidadeNormalizada",    // Usamos a coluna normalizada
                    "CategoriaEncoded"))

                // 6. TREINAR o algoritmo
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: nameof(EventoPrevisaoInput.Participantes),
                    featureColumnName: "Features"));

            // 7. TREINAR O MODELO
            _model = pipeline.Fit(dataView);
        }

        public EventoPrevisaoOutput Predict(EventoPrevisaoInput input)
        {
            if (_model == null)
            {
                throw new InvalidOperationException("O modelo não foi treinado. Chame TrainModel() primeiro.");
            }

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EventoPrevisaoInput, EventoPrevisaoOutput>(_model);
            var prediction = predictionEngine.Predict(input);
            return prediction;
        }

        private static List<EventoPrevisaoInput> GetFakeTrainingData()
        {
            // (Os dados fictícios permanecem os mesmos)
            return new List<EventoPrevisaoInput>
            {
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 1000, Mes = 1, Participantes = 850 },
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 1000, Mes = 7, Participantes = 950 },
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 500, Mes = 11, Participantes = 480 },
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 3000, Mes = 12, Participantes = 2900 },
                new EventoPrevisaoInput { Categoria = "Palestra", CapacidadeMaxima = 100, Mes = 3, Participantes = 80 },
                new EventoPrevisaoInput { Categoria = "Palestra", CapacidadeMaxima = 100, Mes = 10, Participantes = 95 },
                new EventoPrevisaoInput { Categoria = "Palestra", CapacidadeMaxima = 50, Mes = 5, Participantes = 40 },
                new EventoPrevisaoInput { Categoria = "Exposicao", CapacidadeMaxima = 500, Mes = 2, Participantes = 300 },
                new EventoPrevisaoInput { Categoria = "Exposicao", CapacidadeMaxima = 500, Mes = 8, Participantes = 400 },
                new EventoPrevisaoInput { Categoria = "Exposicao", CapacidadeMaxima = 1000, Mes = 12, Participantes = 650 },
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 800, Mes = 4, Participantes = 700 },
                new EventoPrevisaoInput { Categoria = "Palestra", CapacidadeMaxima = 200, Mes = 6, Participantes = 180 },
                new EventoPrevisaoInput { Categoria = "Exposicao", CapacidadeMaxima = 700, Mes = 9, Participantes = 500 },
                new EventoPrevisaoInput { Categoria = "Show", CapacidadeMaxima = 1200, Mes = 10, Participantes = 1100 },
                new EventoPrevisaoInput { Categoria = "Palestra", CapacidadeMaxima = 150, Mes = 1, Participantes = 130 }
            };
        }
    }
}