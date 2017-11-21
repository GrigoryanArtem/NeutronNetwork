using CNTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeutronNetwork.CNN
{
    public delegate void DigitNNEventHandler(DigitNNEventArgs e);

    public class DigitNN
    {
        #region Members

        private int mCurrentTest = 0;
        private DeviceDescriptor device;

        private Digit[] mDigits;
        private Function mFunction;

        private const int INPUT_DIMENSION = 784;
        private const int OUTPUT_DIMENSION = 10;
        private const int HIDDEN_DIMENSION = 1500;

        private NDShape mInputShape;
        private NDShape mOutputShape;

        private Variable mFeatures;
        private Variable mLabels;

        private Parameter w1;
        private Parameter b1;
        private Parameter w2;
        private Parameter b2;

        private Function loss;
        private Function evalError;

        private static readonly Lazy<DigitNN> mLazy =
            new Lazy<DigitNN>(() => new DigitNN());

        #endregion

        #region Properties

        public static DigitNN Instance
        {
            get
            {
                return mLazy.Value;
            }
        }

        #endregion


        public void Learning(string traningFile)
        {
            mDigits = DigitImageCSVReader.Read(traningFile);
            ResetTest();

            TrainingParameterScheduleDouble learningRatePerSample = new TrainingParameterScheduleDouble(0.02, 1);
            var parameterLearners =
                new List<Learner>() { Learner.SGDLearner(mFunction.Parameters(), learningRatePerSample) };
            var trainer = Trainer.CreateTrainer(mFunction, loss, evalError, parameterLearners);

            int minibatchSize = 64;
            int numMinibatchesToTrain = 2000;
            int updatePerMinibatches = 50;

            for (int minibatchCount = 0; minibatchCount < numMinibatchesToTrain; minibatchCount++)
            {
                Value ft, lb;
                GenerateValueData(minibatchSize, INPUT_DIMENSION, OUTPUT_DIMENSION, out ft, out lb, device);
       
                trainer.TrainMinibatch(
                    new Dictionary<Variable, Value>() { { mFeatures, ft }, { mLabels, lb } }, device);

                //TestHelper.PrintTrainingProgress(trainer, minibatchCount, updatePerMinibatches);
            }

            //int count = 0;
            //for(int i = 0; i < 10000; i++)
            //    count += (GetValue(mDigits[i]) == mDigits[i].Label) ? 1 : 0;

        }

        public int GetValue(Digit digit)
        {
            var imap = new Dictionary<Variable, Value> { { mFeatures, Value.CreateBatch<float>(new int[] { INPUT_DIMENSION }, digit.Image, device) } };
            var omap = new Dictionary<Variable, Value> { { mFunction, null } };

            mFunction.Evaluate(imap, omap, device);
            var result = omap[mFunction].GetDenseData<float>(mFunction).First();

            return result.IndexOf(result.Max());
        }

        #region Private methods

        private DigitNN()
        {
            device = DeviceDescriptor.CPUDevice;

            mInputShape = new NDShape(1, INPUT_DIMENSION);
            mOutputShape = new NDShape(1, OUTPUT_DIMENSION);

            mFeatures = Variable.InputVariable(mInputShape, DataType.Float);
            mLabels = Variable.InputVariable(mOutputShape, DataType.Float);

            w1 = new Parameter(new int[] { HIDDEN_DIMENSION, INPUT_DIMENSION }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device, "w1");
            b1 = new Parameter(new int[] { HIDDEN_DIMENSION }, DataType.Float, 0, device, "b1");
            w2 = new Parameter(new int[] { OUTPUT_DIMENSION, HIDDEN_DIMENSION }, DataType.Float, CNTKLib.GlorotUniformInitializer(), device, "w2");
            b2 = new Parameter(new int[] { OUTPUT_DIMENSION }, DataType.Float, 0, device, "b2");

            mFunction = CNTKLib.Times(w2, CNTKLib.ReLU(CNTKLib.Times(w1, mFeatures) + b1)) + b2;

            loss = CNTKLib.CrossEntropyWithSoftmax(mFunction, mLabels);
            evalError = CNTKLib.ClassificationError(mFunction, mLabels);
        }

        private void GenerateValueData(int sampleSize, int inputDim, int numOutputClasses,
            out Value featureValue, out Value labelValue, DeviceDescriptor device)
        {
            float[] features;
            float[] oneHotLabels;
            GenerateRawDataSamples(sampleSize, inputDim, numOutputClasses, out features, out oneHotLabels);

            featureValue = Value.CreateBatch<float>(new int[] { inputDim }, features, device);
            labelValue = Value.CreateBatch<float>(new int[] { numOutputClasses }, oneHotLabels, device);
        }

        private void GenerateRawDataSamples(int sampleSize, int inputDim, int numOutputClasses,
            out float[] features, out float[] oneHotLabels)
        {
            features = new float[sampleSize * inputDim];
            oneHotLabels = new float[sampleSize * numOutputClasses];

            for (int sample = 0; sample < sampleSize; sample++)
            {
                Digit digt = mDigits[(mCurrentTest++) % mDigits.Length];

                for (int i = 0; i < numOutputClasses; i++)
                    oneHotLabels[sample * numOutputClasses + i] = (digt.Label == i) ? 1 : 0;

                for (int i = 0; i < inputDim; i++)
                    features[sample * inputDim + i] = digt.Image[i];
            }
        }

        private void ResetTest()
        {
            mCurrentTest = 0;
        }

        #endregion
    }
}
