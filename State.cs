using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repetitor_on_event
{
    public enum TaskStage
    {
        TS_DESCRIPTION,
        TS_ANSWER
    }

    public partial class State
    {
        public Bitmap StartScreen;
        public Bitmap LogoScreen;

        public Bitmap[] Results;

        public Bitmap[] Transition;

        Tasks TasksStorage;

        int Task = 100;
        int SubTask = 0;

        TaskStage Stage;

        int Result = 0;

        int Steps;
        int Width;
        int Height;

        Random Rnd = new Random();

        public void Load(String folderPrefix, int steps, int width, int height)
        {
            Steps = steps;
            Width = width;
            Height = height;

            StartScreen = new Bitmap(folderPrefix + "/" + "empty_back.png");
            LogoScreen = new Bitmap(folderPrefix + "/" + "repetitor_logo.png");

            Results = new Bitmap[6];
            for (int result = 0; result <= 5; ++result)
            {
                Results[result] = new Bitmap(folderPrefix + "/result_" + result.ToString() + ".png");
            }

            TasksStorage = new Tasks();
            TasksStorage.Load(folderPrefix, Steps, Width, Height);
        }

        public void Reset()
        {
            Task = 100;
            SetupTransition(0);
        }

        public void SetupTransition(int code)
        {
            if (Task == 100)
            {
                Task = -1;
                Result = 0;

                EraseTransition();
                if (Transition == null)
                {
                    Transition = new Bitmap[Steps + 1];
                }
                for (int i = 0; i <= Steps; ++i)
                {
                    Transition[i] = new Bitmap(Width, Height);
                    ImageOps.SetOpacity(LogoScreen, LogoScreen, ref Transition[i], 0, Width, Height);
                }

                return;
            }

            if (Task == 4 && Stage == TaskStage.TS_ANSWER)
            {
                Bitmap startingBitmap = new Bitmap(Transition.Last());
                ImageOps.SetupTransition(startingBitmap, Results[Result], ref Transition, Steps, Width, Height);
                Task = 100;
                startingBitmap.Dispose();
                return;
            }

            if (Task == -1 || Stage == TaskStage.TS_ANSWER)
            {
                NextRandomTask();
                Bitmap startingBitmap = new Bitmap(Transition.Last());
                ImageOps.SetupTransition(startingBitmap, TasksStorage.AllTasks[Task][SubTask].TaskImage, ref Transition, Steps, Width, Height);
                Stage = TaskStage.TS_DESCRIPTION;
                startingBitmap.Dispose();
                return;
            }

            if (Stage == TaskStage.TS_DESCRIPTION)
            {
                if (TasksStorage.AllTasks[Task][SubTask].RightAnswer == code)
                {
                    ++Result;
                }

                Stage = TaskStage.TS_ANSWER;
                TasksStorage.AllTasks[Task][SubTask].GetTransition(code - 1, ref Transition);
                return;
            }
        }

        private void NextRandomTask()
        {
            ++Task;
            SubTask = Rnd.Next() % 5;
            Stage = TaskStage.TS_DESCRIPTION;
        }

        private void EraseTransition()
        {
            if (Transition == null)
            {
                return;
            }
            for (int i = 0; i < Transition.Length; ++i)
            {
                Transition[i].Dispose();
            }
        }
    }
}
