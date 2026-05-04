using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using LemonLaw.Core.Entities;


namespace LemonLaw.Module.Controllers
{
    /// <summary>
    /// Generic list view layout controller applied to all root list views.
    /// Handles:
    ///   1. Dynamic column min-width based on header caption length
    ///   2. Selection column width
    ///   3. Column freezing — configurable per entity type via _frozenColumnCounts
    ///
    /// To freeze columns for a new entity, add an entry to _frozenColumnCounts.
    /// </summary>
    public class ListViewLayoutController : ViewController<ListView>
    {
        /// <summary>
        /// Number of data columns to freeze (left-pin) per entity type.
        /// Selection column is always frozen when any data columns are frozen.
        /// </summary>
        private readonly Dictionary<Type, int> _frozenColumnCounts = new()
        {
            { typeof(VllApplication), 3 },   // Case Number, Application Type, Status
            // Add more entities here as needed:
            // { typeof(LemonLaw.Core.Entities.Hearing), 2 },
        };

        public ListViewLayoutController()
        {
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Root;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            if (View.Editor is not DxGridListEditor gridListEditor)
                return;

            var adapter = gridListEditor.GetGridAdapter();

            // ── 1. Dynamic column min-width based on caption length ────────────
            var visibleColumns = adapter.GridDataColumnModels
                .Where(c => c.Visible)
                .OrderBy(c => c.VisibleIndex)
                .ToList();

            foreach (var column in visibleColumns)
            {
                int charCount = string.IsNullOrEmpty(column.Caption) ? 10 : column.Caption.Length;
                int calculatedWidth = (charCount * 9) + 40;
                column.MinWidth = Math.Max(100, calculatedWidth);
            }

            // ── 2. Selection column width ──────────────────────────────────────
            if (adapter.GridSelectionColumnModel != null)
                adapter.GridSelectionColumnModel.Width = "40px";

            // ── 3. Column freezing ─────────────────────────────────────────────
            var entityType = View.ObjectTypeInfo.Type;

            if (!_frozenColumnCounts.TryGetValue(entityType, out int freezeCount) || freezeCount <= 0)
                return;

            // Freeze selection column
            if (adapter.GridSelectionColumnModel != null)
                adapter.GridSelectionColumnModel.FixedPosition = GridColumnFixedPosition.Left;

            // Freeze first N visible data columns
            foreach (var column in visibleColumns.Take(freezeCount))
                column.FixedPosition = GridColumnFixedPosition.Left;
        }
    }
}
