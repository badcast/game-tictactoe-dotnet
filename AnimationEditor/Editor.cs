using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnimationEditor
{
    public partial class Editor : UserControl
    {

        const string IMAGE_FILTERS = "Все изображения|*.bmp;*.png;*.jpg;*.jpeg;*.tiff;*.ico;*.icon|Точечный рисунок|*.bmp|Portable Network Graphics|*.png|Jpeg|*jpg;*.jpeg|Tagged Image File Format|*.tiff|Иконка Windows|*.ico;*.icon";
        const string ITEMLIST_NAME = "{0} | {1}";
        private AnimationState _animState;
        private int _index;
        private int _start, _end;
        private int _position = -1;
        private OpenFileDialog openFileDialog;
        private bool _isChange = false;
        private Timer _timer;
        private float _startAnimationTime = 0;
        private float _lastTime = 0;
        private bool isBegin = false;
        private bool IsMultiSelected { get { return SelectedIndices.Count > 1; } }
        private ListBox.SelectedIndexCollection SelectedIndices { get => animList.SelectedIndices; }

        public TimeLine TimeLine { get; }
        public Image FrameBackground { get { return screenPlay.BackgroundImage; } set { screenPlay.BackgroundImage = value; } }
        public bool IsLoop { get { return TimeLine.IsLoop; } set { this.chkLoop.Checked = TimeLine.IsLoop = value; } }
        public bool IsPlayingAnimation { get { return this.AnimationState == AnimationState.Playing; } }
        public AnimationState AnimationState { get { return _animState; } set { AnimationToState(value, true); } }
        public int PublicCount { get { return TimeLine.Count; } }
        public int AnimationStartPosition { get { return _start; } }
        public int AnimationPosition { get { return GetAnimationPosition(); } set { SetAnimationPosition(value); } }
        public int AnimationNextPosition { get { return GetNextPosition(); } }
        public int AnimationPreviewPosition { get { return GetPreviewPosition(); } }
        public int AnimationEndPosition { get { return _end; } }
        public int SelectedIndex { get { return GetIndex(); } set { SetIndex(value); } }
        public IPictureProvider SelectedProvider { get { return TimeLine[SelectedIndex]; } }
        public bool IsSelected { get { return SelectedIndices.Count > 0 && SelectedIndex > -1 && SelectedIndex < TimeLine.Count; } }



        public event EventIndexChanged OnAnimationPosChange;
        public Editor()
        {
            InitializeComponent();
            if (DesignMode)
                return;
            this._position = 0;
            this._animState = AnimationState.Stop;
            TimeLine = new TimeLine();
            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Tick += (o, e) => Timing();
            this.progress.Minimum = 0;
            this.progress.Maximum = 0;
        }


        //******LOAD
        private void Editor_Load(object sender, EventArgs eee)
        {

            this._timer.Start();
            this.chkLoop.Checked = IsLoop;
            this.chkLoop.CheckedChanged += (o, e) =>
            {
                if (!chkLoop.Focused)
                    return;

                IsLoop = chkLoop.Checked;
            };

            this.bPlayAnimation.Click += (o, e) =>
            {
                PlayAnimation();

            };

            this.bPauseAnimation.Click += (o, e) =>
            {
                AnimationState = AnimationState.Paused;
            };

            this.bStopAnimation.Click += (o, e) =>
            {
                AnimationState = AnimationState.Stop;
            };

            this.blAdd.Click += (o, e) =>
            {
                Point p = blAdd.Location;
                cm_baddMenu.Show(blAdd, p);
            };

            this.animList.SelectedIndexChanged += (o, e) =>
            {
                SetIndex(animList.SelectedIndex);
            };

            this.animList.MouseDown += (o, ems) =>
            {
                if (PublicCount == 0)
                    return;

                if (ems.Button != MouseButtons.Right)
                    return;
                animList.SelectionMode = SelectionMode.One;
                int sz = animList.Font.Height;
                int h = ems.Y - PublicCount * sz;
                int getIndex = PublicCount-1-Math.Abs(h / sz);
                SetIndex(Math.Max(Math.Min(getIndex, PublicCount - 1), 0));
                animList.SelectionMode = SelectionMode.MultiExtended;
                cm_listMenu.Show(animList, ems.Location);
            };

            this.bResetChanges.Click += (o, e) =>
            {
                _isChange = true;
                RefreshScene();
                PropertyEnabled(true);
            };

            this.peDurationAnime.ValueChanged += (o, e) =>
            {
                if (!peDurationAnime.Focused)
                    return;
                if (!IsSelected)
                {
                    peDurationAnime.Value = (decimal)Picture.DefaultDuration.TotalSeconds;
                    return;
                }

                _isChange = true;
                PropertyEnabled(true);
            };

            this.peNameAnime.TextChanged += (o, e) =>
            {
                if (!peNameAnime.Focused)
                    return;

                if (!IsSelected)
                {
                    peNameAnime.Text = "(Объект не выбран)";
                    return;
                }
                _isChange = true;
                PropertyEnabled(true);
            };

            this.bApplyChanges.Click += (o, e) =>
            {
                ApplyChanges();
            };

            this.bCancelChanges.Click += (o, e) =>
            {
                CancelChanges();
            };

            this.progress.ValueChanged += (o, e) =>
            {
                if (progress.Focused)
                {
                    if (IsPlayingAnimation)
                        AnimationToState(AnimationState.Stop, false);

                    AnimationPosition = AnimationStartPosition + progress.Value;
                }

            };

            this.blDelete.Click += (o, e) =>
            {
                if (!IsSelected)
                {
                    MessageBox.Show("Ошибка удаления.\n\tОбъект не выбран.");
                    return;
                }
                int lastIndex = SelectedIndex;

                int[] indexes = new int[SelectedIndices.Count];
                SelectedIndices.CopyTo(indexes, 0);
                int justDelete = 0;
                for (int i = 0; i < indexes.Length; i++)
                {
                    RemoveAt(indexes[i] - justDelete, false);
                    justDelete++;
                }

                RefreshScene();

                SelectedIndex = lastIndex >= PublicCount - 1 ? PublicCount - 1 : lastIndex;
            };

            this.bToClone.Click += (o, e) =>
            {
                if (!IsSelected && !IsMultiSelected)
                {
                    MessageBox.Show("Объект(ы) для клонирования не выбраны.");
                    return;
                }

                int clonedCount = 0;
                string addMsg = "";
                if(IsMultiSelected)
                {
                    int startIndex = SelectedIndices[0];
                    int end = SelectedIndices[SelectedIndices.Count - 1];
                    addMsg = "\n";
                    var clns = CloneObjects(startIndex, end);
                    clonedCount = clns.Length;
                    int lTo = Math.Min(clonedCount, 5);
                    for (int i = 0; i < lTo; i++)
                    {
                        addMsg += string.Format("\n\t{0} - {1}", i+1, clns[i].Name);
                    }
                    if (clonedCount > 5)
                        addMsg += "\n\t...";
                }
                else
                {
                    CloneObject(SelectedIndex);
                    clonedCount = 1;
                }


                MessageBox.Show(string.Format("{0} объект(ов) клонирован(о)." + addMsg, clonedCount));
            };
            MouseEventHandler ___pointer = null;
            ___pointer += (o, eec) =>
            {
                if (!IsSelected || eec.Button != MouseButtons.Right)
                    return;
                mmc_inbeg.Visible = o == blToUp;
                mmc_inend.Visible = o == blToDown;

                cm_toPointer.Show((o as Control), new Point() + (o as Control).Size);
            };
            this.blToUp.Click += (o, e) =>
            {
                MoveToIndex(SelectedIndex, SelectedIndex - 1);
            };
            this.blToUp.MouseDown += ___pointer;

            this.blToDown.Click += (o, e) =>
            {
                MoveToIndex(SelectedIndex, SelectedIndex + 1);
            };
            this.blToDown.MouseDown += ___pointer;


            //MENUS
            mc_addFiles.Click += (o, e) => AddShotOpenDialog();
            mmc_inbeg.Click += (o, e) => MoveToIndex(SelectedIndex, 0);
            mmc_inend.Click += (o, e) => MoveToIndex(SelectedIndex, PublicCount-1);

            this.AnimationToState(AnimationState.Idle, true);

            this.RefreshScene();
        }

        //****Private fields
        private void AnimationToState(AnimationState value, bool invoke)
        {
            _animState = value;

            if (value == AnimationState.Playing && invoke)
                StartAnimation(IsLoop);
            else
            if (value == AnimationState.Paused && invoke)
                PauseAnimation();
            else if ((value == AnimationState.Stop || value == AnimationState.Idle) && invoke)
                StopAnimation();

            animStatePanel.Enabled = value != AnimationState.Idle;
            bPauseAnimation.Enabled = value != AnimationState.Paused && value != AnimationState.Stop;
            bPlayAnimation.Enabled = value == AnimationState.Stop || value == AnimationState.Paused;
            bStopAnimation.Enabled = value == AnimationState.Playing || value == AnimationState.Paused;
        }

        private void PropertyEnabled(bool enabled)
        {
            bApplyChanges.Enabled = enabled;
            bCancelChanges.Enabled = enabled;
            bResetChanges.Enabled = enabled;
        }

        private void RefreshScene(bool refreshList = false)
        {
            PropertyEnabled(_isChange);

            if (PublicCount == 0)
            {
                AnimationState = AnimationState.Idle;
                screenPlay.Image = null;
            }
            else if (PublicCount > 0)
            {
                if (AnimationEndPosition == 0)
                {
                    StartAnimation(0, PublicCount - 1, IsLoop);
                    StopAnimation();
                    AnimationState = AnimationState.Stop;
                }
            }

            AnimationPosition = AnimationPosition;

            if (animList.Items.Count != TimeLine.Count || refreshList)
            {
                animList.Items.Clear();
                for (int i = 0; i < TimeLine.Count; i++)
                {
                    IPictureProvider prov = TimeLine[i];
                    animList.Items.Add(string.Format(ITEMLIST_NAME, i + 1, prov.Name));
                }
            }

            if (!IsSelected)
            {
                peNameAnime.Text = "";
                return;
            }



            peNameAnime.Text = SelectedProvider.Name;
            peDurationAnime.Value = (decimal)SelectedProvider.Duration.TotalSeconds;
            pCadrSizeHorizontal.Text = "Горизонтальный размер: " + (SelectedProvider as Picture).Width;
            pCadrSizeVertical.Text = "Вертикальный размер: " + (SelectedProvider as Picture).Height;
        }

        private void ApplyTo(IPictureProvider provider, string nameValue, TimeSpan durationValue)
        {
            provider.Name = peNameAnime.Text;
            provider.Duration = durationValue;
        }

        private void ApplyChanges()
        {
            if (!_isChange)
                return;
            _isChange = false;

            string stringValue = peNameAnime.Text;
            TimeSpan durationValue = TimeSpan.FromSeconds((double)peDurationAnime.Value);
            int[] indexes = new int[SelectedIndices.Count];
            SelectedIndices.CopyTo(indexes, 0);
            for (int i = 0; i < indexes.Length; i++)
            {
                int index = indexes[i];
                ApplyTo(TimeLine[index], stringValue, durationValue);
                animList.Items[index] = string.Format(ITEMLIST_NAME, index + 1, stringValue);
            }
            PropertyEnabled(false);
            RefreshScene();
        }

        private void CancelChanges()
        {
            if (!_isChange)
                return;
            _isChange = false;
            RefreshScene();
            PropertyEnabled(false);
        }

        private bool AnimationIsRange(int animPosition)
        {
            return AnimationIsRange(animPosition, _start, _end);
        }
        private bool AnimationIsRange(int animPosition, int _start, int _end)
        {
            return !(_start - _end > 0 || animPosition < _start || animPosition > _end);
        }

        private int GetNextPosition()
        {
            int havent = AnimationPosition + 1;

            if (!AnimationIsRange(havent))
                if (IsLoop)
                    return _start;
                else
                    return -1;

            return havent;
        }

        private int GetPreviewPosition()
        {
            int havent = AnimationPosition - 1;

            if (!AnimationIsRange(havent, AnimationStartPosition, AnimationEndPosition))
                if (IsLoop)
                    return PublicCount - 1;
                else
                    return -1;

            return havent;
        }

        private void Timing()
        {
            if (IsPlayingAnimation)
            {
                if (!(TimeLine.IsRange(AnimationPosition) || TimeLine.IsRange(GetNextPosition())))
                {
                    StopAnimation();
                    return;
                }
                if (isBegin)
                {
                    AnimationPosition = _start;
                    _startAnimationTime = Time.time;
                    _lastTime = Time.time + (float)TimeLine[AnimationPosition].Duration.TotalSeconds;
                    isBegin = false;
                }


                if (Time.time > _lastTime)
                {
                    int next = GetNextPosition();

                    if (IsLoop)
                    {

                    }

                    IPictureProvider nextProvider = TimeLine[next];
                    if (nextProvider == null)
                    {
                        StopAnimation();
                        return;
                    }
                    AnimationPosition = next;
                    _lastTime = Time.time + (float)nextProvider.Duration.TotalSeconds;
                }

            }
            else if (!isBegin && AnimationState != AnimationState.Paused)
            {
                isBegin = true;
                _startAnimationTime = 0;
            }

            iFps.Text = Time.fps + " FPS";
            Point mspoint = new Point(MousePosition.X - screenPlay.Location.X, MousePosition.Y - screenPlay.Location.Y);

            if (mspoint.X < 0)
                mspoint.X = 0;
            else if (mspoint.X > screenPlay.Width)
                mspoint.X = screenPlay.Width;
            if (mspoint.Y < 0)
                mspoint.Y = 0;
            else if (mspoint.Y > screenPlay.Height)
                mspoint.Y = screenPlay.Height;

            iMouseXY.Text = string.Format("Точка {0}x{1}", mspoint.X, mspoint.Y);
            Time.CalculateFps();

        }

        private Picture __cloneNormal(Picture dest)
        {
            Picture clone = new Picture(dest.GetBitmap(), true);
            clone.Name = Picture.DefaultName + " (Clone)";
            clone.Duration = dest.Duration;
            return clone;
        }

        //****Public fields

        public int GetIndex()
        {
            return _index;
        }

        public void SetIndex(int index)
        {
            _isChange = false;
            if (!TimeLine.IsRange(index))
                return;

            _index = index;
            StopAnimation();
            AnimationPosition = index;
            TimeLine.SetIndex(index);
            animList.SelectedIndex = index;
            RefreshScene();
        }

        public void RemoveAt(int index, bool canUpdate = true)
        {
            StopAnimation();

            if (TimeLine.RemoveAt(index) && canUpdate)
            {
                AnimationToState(AnimationState.Idle, false);
                RefreshScene();
            }
        }

        public IPictureProvider CloneObject(int index)
        {
            StopAnimation();

            if (!TimeLine.IsRange(index))
                return null;

            Picture p = (Picture)__cloneNormal((Picture)TimeLine[index]);
            AddShot(p);
            return p;
        }

        public IPictureProvider[] CloneObjects(int startIndex, int endIndex)
        {
            StopAnimation();

            if (!TimeLine.IsRange(startIndex) || !TimeLine.IsRange(endIndex))
                return null;

            if (startIndex - endIndex >= 0)
                throw new ArgumentOutOfRangeException();

            IPictureProvider[] __clones = new IPictureProvider[Math.Abs(endIndex - startIndex)+1];
            for (int i = 0; i < __clones.Length; i++)
            {
                Picture clone = __cloneNormal((Picture)TimeLine[startIndex+i]);
                __clones[i] = clone;
                TimeLine.Add(clone);
            }
            RefreshScene();
            return __clones;
        }

        public void MoveToIndex(int from, int to)
        {
            StopAnimation();

            if (TimeLine.Move(from, to))
            {
                RefreshScene(true);
                SetIndex(to);
            }
        }

        public void AddShot(Picture picture)
        {
            StopAnimation();
            TimeLine.Add(picture);
            RefreshScene();
        }

        public void AddShotFromFile(string FileName)
        {
            AddShot(Picture.CreateFromFile(FileName, false));
        }

        public void AddShotOpenDialog()
        {
            if (openFileDialog == null)
                openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = IMAGE_FILTERS;

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;


            StopAnimation();

            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                Picture p = Picture.CreateFromFile(openFileDialog.FileNames[i], false);
                TimeLine.Add(p);
            }
            RefreshScene();
        }

        public int GetAnimationPosition()
        {
            return _position;
        }

        public void PlayAnimation()
        {
            if (PublicCount == 0)
                return;

            if (AnimationState == AnimationState.Paused)
            {
                int temp = AnimationPosition;
                StartAnimation(AnimationStartPosition, AnimationEndPosition, IsLoop);
                SetAnimationPosition(temp);
            }
            else if (AnimationState != AnimationState.Playing)
            {
                int startPos = 0;
                int endPos = PublicCount - 1;
                if (IsMultiSelected)
                {
                    startPos = SelectedIndices[0];
                    endPos = SelectedIndices[SelectedIndices.Count - 1];
                }

                StartAnimation(startPos, endPos, IsLoop);
            }
        }

        public void StartAnimation(bool loop)
        {
            if (PublicCount == 0)
                return;

            StartAnimation(0, PublicCount - 1, loop);
        }

        public void StartAnimation(int start, int end, bool isLoop)
        {
            if (!AnimationIsRange(start, start, end))
                throw new ArgumentOutOfRangeException();


            StopAnimation();

            AnimationToState(AnimationState.Playing, false);

            this._start = start;
            this._end = end;
            this.progress.Minimum = 0;
            this.progress.Maximum = AnimationEndPosition - AnimationStartPosition;
            this.IsLoop = isLoop;
        }

        public void PauseAnimation()
        {
            AnimationToState(AnimationState.Paused, false);

        }

        public void SetAnimationPosition(int pos)
        {
            if (!AnimationIsRange(pos))
                return;
            if (!(TimeLine.IsRange(pos)))
            {
                StopAnimation();
                return;
            }

            int last = AnimationPosition;
            _position = pos;
            screenPlay.Image = TimeLine[AnimationPosition].GetBitmap();

            progress.Value = pos - AnimationStartPosition;
            OnAnimationPosChange?.Invoke(this, AnimationPosition, last);
        }

        bool ___blockStop = false;
        public void StopAnimation()
        {
            if (___blockStop)
                return;
            ___blockStop = true;
            AnimationToState(AnimationState.Stop, false);
            SetAnimationPosition(AnimationStartPosition);
            ___blockStop = false;
        }
    }
}
