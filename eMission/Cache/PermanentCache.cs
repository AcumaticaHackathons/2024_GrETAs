using PX.Data;

namespace eMission.Cache
{
    /// <summary>
    ///   The purpose of this class is create a cache replacement that doesn't allow the graph to clear it.
    /// </summary>
    public class PermanentCache<Table> : PXCache<Table> where Table : class, IBqlTable, new()
    {
        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="graph"></param>
        public PermanentCache(PXGraph graph) : base(graph) { }
    
        /// <summary>
        ///   Clear function - do nothing
        /// </summary>
        public override void Clear() { }
        
        /// <summary>
        ///   Persist function - do nothing
        /// </summary>
        public override int Persist(PXDBOperation operation) {
            return 0;
        }

        /// <summary>
        ///   IsDirty - always false
        /// </summary>
        public override bool IsDirty { get; set; } = false;
    }
}