using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace BotMapLevel
{
    public partial class Form1 : Form
    {
        public class Matrix2x2 : ICloneable
        {
            private int[] _mtrx;
            private Matrix2x2 _sync = null, template;
            private int size;
            private bool _isSync = false, isSourceSync;

            public int this[int index]
            {
                get
                {
                    if (IsSynchronized)
                        return IndexOfSync(index);

                    return IndexOf(index);
                }
                set
                {
                    SetValue(index, value);
                }
            }
            public int this[int x, int y]
            {
                get
                {
                    if (IsSynchronized)
                        return IndexOfSync(x, y);
                    return IndexOf(x, y);
                }
                set
                {
                    SetValue(x, y, value);
                }
            }
            public int this[Point point]
            {
                get
                {
                    if (IsSynchronized)
                        return IndexOfSync(point);
                    return IndexOf(point);
                }
                set
                {
                    SetValue(point, value);
                }
            }
            /// <summary>
            /// Определяет, матрица только для чтения
            /// </summary>
            public bool IsReadOnly { get; }

            public int AmmountField
            {
                get
                {
                    int fieldCount = 0;
                    for (int i = 0; i < Length; i++)
                    {
                        if (_mtrx[i] != DefaultValues.TYPE_STEP_DEFAULT)
                            fieldCount++;
                    }
                    return fieldCount;
                }
            }
            public int SyncAmmountField
            {
                get
                {
                    if (!IsSynchronized)
                        return -1;

                    int fieldCount = 0;
                    Matrix2x2 mx = SynchronizeMatrix;
                    for (int i = 0; i < mx.Length; i++)
                    {
                        if (mx[i] != DefaultValues.TYPE_STEP_DEFAULT)
                            fieldCount++;
                    }
                    return fieldCount;
                }

            }

            /// <summary>
            /// Определяет, результат синхронизации матрицы
            /// </summary>
            public bool IsSynchronized { get { return _isSync; } }
            public bool IsSourceSync { get { return isSourceSync; } }
            /// <summary>
            /// Матрица, которое лежит на основе (Синхронизированного).
            /// </summary>
            public Matrix2x2 SynchronizeMatrix { get { return GetMatrixSync(); } }
            /// <summary>
            /// Провереят, совместимый ли размер из синхронизаций
            /// </summary>
            public bool IsSupportedSynchronize { get { if (!IsSynchronized) throw new NullReferenceException(); return _sync.Size == Size; } }
            /// <summary>
            /// Реальный размер массива
            /// </summary>
            public int Length { get { return _mtrx.Length; } }
            /// <summary>
            /// Квадратный размер матрицы
            /// </summary>
            public int Size { get { if (size == 0) size = (int)Math.Sqrt(_mtrx.Length); return size; } }
            public Matrix2x2(int SquareSize, bool IsReadOnly = false)
            {
                if (SquareSize <= 1)
                    throw new ArgumentException();

                this.IsReadOnly = IsReadOnly;
                _mtrx = new int[(int)Math.Pow(SquareSize, 2)];
            }
            public Matrix2x2(int[] sqrtArray, bool IsReadOnly = false)
            {

                int len = sqrtArray.Length;
                int wid = len / 2;
                int normalSize = wid * 2;

                if (len - normalSize != 0)
                    throw new ArgumentException();

                this.IsReadOnly = IsReadOnly;

                _mtrx = new int[len];
                for (int x = 0; x < len; x++)
                {
                    _mtrx[x] = sqrtArray[x];
                }
            }
            /// <summary>
            /// Синхронизирует с нижнего уровня
            /// </summary>
            public void Synchronize(Matrix2x2 sync)
            {
                if (this.IsReadOnly)
                    throw new ReadOnlyException();

                if (sync == null)
                    throw new ArgumentNullException();

                if (sync == this)
                    throw new DuplicateNameException();
                //Объявляем ее как главный источник синхронизаций
                if (false && !sync._isSync)
                {
                    sync.isSourceSync = true;
                    sync._isSync = true;
                    sync._sync = sync;
                }
                this._sync = sync;
                this._isSync = true;
            }
            /// <summary>
            /// Отключает синхронизацию 
            /// </summary>
            public void UnSynchronize()
            {
                if (!IsSynchronized)
                    return;

                _sync = null;
                _isSync = false;
            }
            public Matrix2x2 GetMatrixSync()
            {
                if (!IsSynchronized)
                    return null;

                void getTemplate(Matrix2x2 destination, int[] data)
                {
                    int def = DefaultValues.TYPE_STEP_DEFAULT;
                    for (int i = 0; i < data.Length; i++)
                    {
                        int fValue = data[i];
                        int mValue = _mtrx[i];
                        int sValue = destination._mtrx[i];
                        int toVal = def;

                        //Условия проходимости
                        if (mValue == def && sValue != def)
                            toVal = sValue;
                        else if (sValue == def && mValue != def)
                            toVal = mValue;
                        else
                            toVal = fValue;

                        //Устанавливаем значение для синхронизаций
                        data[i] = toVal;
                    }
                }

                //Если шаблон пустой, то создаем его
                if (template == null)
                    template = (Matrix2x2)this.Clone();
                template.Reset();

                Matrix2x2 sel = this;
                int[] df = template._mtrx;
                while (sel != null)
                {
                    getTemplate(sel, df);
                    sel = sel._sync;
                }

                return template;
            }
            /// <summary>
            /// Сбрасывает матрицу по умолчанию
            /// </summary>
            public void Reset()
            {
                if (IsReadOnly)
                    throw new ReadOnlyException();

                for (int i = 0; i < _mtrx.Length; i++)
                {
                    _mtrx[i] = DefaultValues.TYPE_STEP_DEFAULT;
                }
            }
            public void SetValue(int index, int value)
            {
                if (IsReadOnly)
                    throw new ReadOnlyException();

                _mtrx[index] = value;
            }
            public void SetValue(int x, int y, int value)
            {
                if (IsReadOnly)
                    throw new ReadOnlyException();

                _mtrx[x * Size + y] = value;
            }
            public void SetValue(Point point, int value)
            {
                SetValue(point.X, point.Y, value);
            }
            public int IndexOfSync(Point point)
            {
                return IndexOfSync(point.X, point.Y);
            }
            public int IndexOfSync(int x, int y)
            {
                if (!IsSynchronized)
                    throw new NullReferenceException();

                return GetMatrixSync()[x, y];
            }
            public int IndexOfSync(int index)
            {
                if (!IsSynchronized)
                    throw new NullReferenceException();
                return GetMatrixSync()[index];
            }
            public int IndexOf(Point point)
            {
                return IndexOf(point.X, point.Y);
            }
            public int IndexOf(int x, int y)
            {
                return _mtrx[x * Size + y];
            }
            public int IndexOf(int index)
            {
                return _mtrx[index];
            }
            public int[] ToArray()
            {
                return (int[])_mtrx.Clone();
            }
            public int[] ToArraySync()
            {
                if (!IsSynchronized)
                    throw new NullReferenceException();
                return GetMatrixSync().ToArray();
            }

            /// <summary>
            /// Копирует содержмимое в новый <see cref="Matrix2x2"/> без синхронизаций
            /// </summary>
            /// <returns></returns>
            public object Clone()
            {
                Matrix2x2 clone = new Matrix2x2(this.Size);

                for (int i = 0; i < _mtrx.Length; i++)
                {
                    clone._mtrx[i] = _mtrx[i];
                }

                return (object)clone;
            }
        }
        public class MapStep
        {
            private MapStepData data;
            private bool isLocated;
            private MapStep[] _childs;
            private Matrix2x2 _mtrx;
            private bool hasChilds;
            private int _id;
            private int _lUpId;

            /// <summary>
            /// Матрицу
            /// </summary>
            public Matrix2x2 Matrix { get { return _mtrx; } }

            public int AmmountField { get => _mtrx.AmmountField; }
            public int SuncAmmountField { get => _mtrx.SyncAmmountField; }

            public int ID { get => _id; }
            public bool IsGlobalAllocated { get => data.IsAllocated(this); }
            public MapStepData Owner { get => data; }
            public bool IsRoot { get => Root == this; }
            public MapStep Root { get => data?.Root; }
            public MapStep LevelUp { get { return data.FromId(_lUpId); } }
            public bool HasChilds { get { Alloc(); return hasChilds; } }
            public MapStep[] Childs { get => GetChilds(); }
            public MapStep(MapStepData data, MapStep levelUp, Matrix2x2 matrix, int id)
            {
                if (matrix == null)
                    throw new ArgumentNullException();
                this.data = data;
                if (levelUp == null)
                    _lUpId = -1;
                else
                    _lUpId = levelUp.ID;
                this._id = id;
                this._mtrx = matrix;
            }
            private void Alloc()
            {
                if (data == null)
                    throw new NullReferenceException();

                if (data.IsAllocated(this) && isLocated)
                    return;
                data.Alloc(this);
                isLocated = true;

                _childs = data.GetChildsFrom(this);
                hasChilds = _childs != null && _childs.Length > 0;
            }
            public MapStep[] GetChilds()
            {
                Alloc();
                return _childs;
            }

        }
        public class MapStepData
        {
            private List<MapStep> maps;
            private List<bool> mapAllocs;
            private List<List<int>> ids;
            private MapStep __root;

            public MapStep Root { get => __root; }
            public MapStepData()
            {
                maps = new List<MapStep>();
                mapAllocs = new List<bool>();
                ids = new List<List<int>>();
                __root = new MapStep(this, null, new Matrix2x2(3, true), 0);
                maps.Add(Root);
                ids.Add(new List<int>(1024));
                mapAllocs.Add(false);
            }
            private void AddId(int src, int idVal)
            {
                List<int> from = GetIdChilds(src);
                if (from == null)
                {
                    from = ids[src] = new List<int>();
                }
                from.Add(idVal);
            }
            private List<int> GetIdChilds(int src)
            {
                List<int> res = null;
                while (ids.Count <= src)
                {
                    ids.Add(new List<int>());
                }
                res = ids[src];
                return res;
            }
            private void DeleteId(int src, int idVal)
            {
                List<int> from = ids[src];
                if (from == null)
                {
                    from = ids[src] = new List<int>();
                    return;
                }
                from.Remove(idVal);
            }
            private void UnAlloc(MapStep local)
            {
                MapStep finder = local;
                while (finder != null)
                {
                    mapAllocs[finder.ID] = false;
                    finder = finder.LevelUp;
                }
            }
            /// <summary>
            /// Добавляет матрицу в локальную базу
            /// </summary>
            /// <returns>Результат от Matrix</returns>
            public MapStep Add(MapStep local, Matrix2x2 matrix)
            {
                int quartSize = matrix.Size;

                int id = maps.Count;
                MapStep ms = new MapStep(this, local, matrix, id);
                maps.Add(ms);
                mapAllocs.Add(false);
                AddId(local.ID, ms.ID);
                UnAlloc(ms);
                return ms;
            }
            public bool IsAllocated(MapStep local)
            {
                if (local.IsRoot)
                    return false;
                bool allocRes = mapAllocs[local.ID];
                return allocRes;
            }
            public void Alloc(MapStep local)
            {
                if (local.IsRoot)
                    return;

                mapAllocs[local.ID] = true;
            }
            public MapStep FromId(int id)
            {
                if (id == -1)
                    return null;
                return maps[id];
            }
            public bool HasChildsFrom(MapStep location)
            {
                return GetChildsFrom(location)?.Length > 0;
            }
            public MapStep[] GetChildsFrom(MapStep location)
            {

                MapStep[] childs = null;
                List<int> idChilds = GetIdChilds(location.ID);
                if (idChilds.Count > 0)
                {
                    childs = new MapStep[idChilds.Count];
                    for (int i = 0; i < childs.Length; i++)
                    {
                        childs[i] = maps[idChilds[i]];
                    }
                }

                return childs;
            }

            public void Save(string filename)
            {

                void write(BinaryWriter bw, MapStep target)
                {
                    bw.Write(target.ID);

                    Matrix2x2 mx = target.Matrix;
                    for (int i = 0; i < mx.Length; i++)
                    {
                        bw.Write(mx[i]);
                    }

                }

                using (BinaryWriter bw = new BinaryWriter(File.Create(filename)))
                {
                    int mpCount = maps.Count;
                    bw.Write(mpCount);
                    int mtrxCount = this.Root.Matrix.Length;
                    bw.Write(mtrxCount);

                    for (int i = 0; i < mpCount; i++)
                    {
                        MapStep cur = maps[i];
                        int lvUpId = -1;
                        if(cur.LevelUp != null)
                            lvUpId = cur.LevelUp.ID;
                        bw.Write(lvUpId);
                        write(bw, cur);
                        //Write childs
                        List<int> idchld = GetIdChilds(i);
                        bw.Write(idchld.Count);
                        for (int c = 0; c < idchld.Count; c++)
                        {
                            bw.Write(idchld[c]);
                        }
                    }


                }

            }

            public void Load(string filename)
            {
                if (!File.Exists(filename))
                    return;

                MapStep read(BinaryReader br, int id, MapStep levUp)
                {
                    //read id
                    br.ReadInt32();
                    Matrix2x2 mx = new Matrix2x2(this.Root.Matrix.Size);
                    int l = mx.Length;
                    for (int i = 0; i < l; i++)
                    {
                        mx[i] = br.ReadInt32();
                    }
                    return new MapStep(this, levUp, mx, id);
                }

                this.ids.Clear();
                this.maps.Clear();
                this.mapAllocs.Clear();

                using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
                {
                    int mpCount = br.ReadInt32();
                    int mtrxCount = br.ReadInt32();

                    for (int i = 0; i < mpCount; i++)
                    {
                        int lvUpId = br.ReadInt32();
                        MapStep r = read(br, i, lvUpId == -1 ? null : maps[lvUpId]);
                        // read childs
                        int childCount = br.ReadInt32();
                        ids.Add(new List<int>());
                        for (int c = 0; c < childCount; c++)
                        {
                            AddId(i, br.ReadInt32());
                        }
                        maps.Add(r);
                        mapAllocs.Add(false);
                    }
                }

                maps[0] = __root;

            }
        }

        public MapStepData mapStepData;
        private List<Button> fields;
        private string[] strings =
        {
            "Укажите первоначальные расположение.",
            "Укажите следующие расположение"
        };
        private Map map;
        private int currentStepType = 1;
        private int level = 0;
        private int step = 0;
        private bool isBegan = false;
        private MapStep CurrentMapStep { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            map = new Map(3, 3);
            mapStepData = new MapStepData();
            fields = new List<Button>()
            {
                b0,
                b1,
                b2,
                b3,
                b4,
                b5,
                b6,
                b7,
                b8
            };
            listBox1.DoubleClick += (o, ee) =>
            {
                int sel = listBox1.SelectedIndex;
                if (sel == -1)
                    return;

                bool isRoot = CurrentMapStep.IsRoot;
                if (sel == 0)
                {

                    if (!isRoot)
                    {
                        Selected(CurrentMapStep.LevelUp);
                    }
                    else
                        Selected(CurrentMapStep);
                }
                else if (CurrentMapStep.HasChilds)
                {
                    MapStep cur = CurrentMapStep.Childs[sel - 1];
                    Selected(cur);
                }
            };

            comboBox1.SelectedIndex = currentStepType;
            comboBox1.SelectedIndexChanged += (o, ee) =>
            {
                currentStepType = comboBox1.SelectedIndex;
            };


            //Выбераем раздел по умолчанию
            Selected(mapStepData.Root);
        }

        private void butClick_Click(object sender, EventArgs e)
        {
            Button sel = sender as Button;
            int selIndex = (int.Parse(sel.Tag?.ToString()));
            ToDefineStep(selIndex);
        }
        private void DrawListBox()
        {
            nameObject.Text = "Имя элемента: Элемент-" + (CurrentMapStep.ID + 1);
            IsReadOnlyObject.Text = "Объект только для чтения: " + (CurrentMapStep.Matrix.IsReadOnly ? "Да" : "Нет");
            idObject.Text = "id: " + CurrentMapStep.ID;
            LevelUpObject.Text = CurrentMapStep.IsRoot ? "Корневой" : "Элемент верх. уровня: id=" + CurrentMapStep.LevelUp.ID;
            ChildCountObject.Text = "Количество дочерных: " + (CurrentMapStep.HasChilds ? CurrentMapStep.Childs.Length : 0);
            IsSyncObject.Text = "Синхронизирован с родителем: " + (CurrentMapStep.Matrix.IsSynchronized ? "Да" : "Нет");
            infoIsOverrided.Text = CurrentMapStep.AmmountField > 0 ? "Игрок определен." : "Игрок не определен.";
            _labelMap.Text = "Выбранный элемент: id=" + CurrentMapStep.ID;
            listBox1.Items.Clear();
            listBox1.Items.Add(CurrentMapStep.IsRoot ? ".::Корень::." : "<= Назад");
            if (CurrentMapStep.HasChilds)
            {
                var c = CurrentMapStep.Childs;
                for (int i = 0; i < c.Length; i++)
                {
                    listBox1.Items.Add("Под элемент id=" + c[i].ID);
                }
            }
            listBox1.SelectedIndex = 0;
        }
        private void Selected(MapStep select)
        {
            CurrentMapStep = select;
            DrawListBox();
            DrawLogic(select);
        }
        private void DrawLogic(MapStep draw)
        {
            Matrix2x2 selMatrx = draw.Matrix.IsSynchronized ? draw.Matrix.GetMatrixSync() : draw.Matrix;
            for (int i = 0; i < fields.Count; i++)
            {
                Button index = fields[i];
                int stp = (isSyncCheck.Checked) && selMatrx.IsSynchronized ? selMatrx.IndexOfSync(i) : selMatrx.IndexOf(i);
                index.Text = stp == 1 ? "x" : stp == 2 ? "0" : "";
            }
        }
        private void DoMap(MapStep change, Map map)
        {

            Matrix2x2 mtrx2x2 = change.Matrix;
            Cell[] cc = map.ToArray();
            for (int i = 0; i < cc.Length; i++)
            {
                Cell c = cc[i];
                mtrx2x2.SetValue(c.X, c.Y, c.StepType);
            }
        }
        private MapStep MakeMap(MapStep local, Map map)
        {
            Matrix2x2 mtrx2x2 = new Matrix2x2(local.Matrix.Size);
            var mp = mapStepData.Add(local, mtrx2x2);
            DoMap(mp, map);
            mp.Matrix.Synchronize(local.Matrix);
            return mp;
        }
        private void ResetMap()
        {
            map.Reset();
        }
        private void ToDefineStep(int selIndex)
        {
            if (CurrentMapStep.Matrix.IsReadOnly)
            {
                MessageBox.Show("Объект только для чтения.");
                return;
            }
            Cell[] cells = map.ToArray();


            Cell selected = cells[selIndex];
            selected.StepType = currentStepType;
            DoMap(CurrentMapStep, map);
            Selected(CurrentMapStep);
            ResetMap();
        }
        private void Next()
        {
            if (isBegan)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MakeMap(CurrentMapStep, map);
            Selected(CurrentMapStep);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Selected(this.mapStepData.Root);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (CurrentMapStep.IsRoot)
                return;
            Selected(CurrentMapStep.LevelUp);
        }

        private void button12_Click(object sender, EventArgs e)
        {


            int ind = listBox1.SelectedIndex;
            if (ind == -1)
                return;
            if (ind == 0 && !CurrentMapStep.IsRoot)
            {
                Selected(CurrentMapStep.LevelUp);
            }
            else if (ind > 0 && CurrentMapStep.HasChilds)
                Selected(CurrentMapStep.Childs[ind - 1]);
        }

        OpenFileDialog op;
        private void button2_Click(object sender, EventArgs e)
        {
            if (op == null)
            {
                op = new OpenFileDialog();
                op.Filter = "Logic file(*.lgc)|*.lgc";
            }

            if (op.ShowDialog() != DialogResult.OK)
                return;

            mapStepData.Load(op.FileName);
            Selected(mapStepData.Root);
        }

        SaveFileDialog sd;
        private void button3_Click(object sender, EventArgs e)
        {
            if (sd == null)
            {
                sd = new SaveFileDialog();
                sd.Filter = "Logic file(*.lgc)|*.lgc";
            }

            if (sd.ShowDialog() != DialogResult.OK)
                return;

            mapStepData.Save(sd.FileName);
        }
    }
}
