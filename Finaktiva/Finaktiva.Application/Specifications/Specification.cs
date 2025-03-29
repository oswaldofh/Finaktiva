using Finaktiva.Application.Contracts.ISpecifications;
using System.Linq.Expressions;

namespace Finaktiva.Application.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        private List<Expression<Func<T, bool>>> _criteria = new List<Expression<Func<T, bool>>>();

        public Specification()
        {
            // Inicializa con un criterio que no filtra nada
            _criteria.Add(x => true);
        }

        public Specification(Expression<Func<T, bool>> criteria)
        {
            _criteria.Add(criteria ?? (x => true));
        }

        public Expression<Func<T, bool>> Criteria
        {
            get
            {
                // Combinar todos los criterios con AND
                Expression<Func<T, bool>> combined = x => true;

                foreach (var criteria in _criteria)
                {
                    combined = CombineExpressions(combined, criteria, Expression.AndAlso);
                }
                return combined;
            }
        }

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            if (criteria != null)
            {
                _criteria.Add(criteria);
            }
        }

        private static Expression<Func<T, bool>> CombineExpressions(
            Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second,
            Func<Expression, Expression, BinaryExpression> combiner)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var firstVisitor = new ReplaceExpressionVisitor(first.Parameters[0], parameter);
            var firstBody = firstVisitor.Visit(first.Body);

            var secondVisitor = new ReplaceExpressionVisitor(second.Parameters[0], parameter);
            var secondBody = secondVisitor.Visit(second.Body);

            return Expression.Lambda<Func<T, bool>>(
                combiner(firstBody, secondBody), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }

        // Resto de tu implementación...
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public List<string> IncludeStrings { get; } = new();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
        protected void AddInclude(string includeString) => IncludeStrings.Add(includeString);
    }
    /*public class Specification<T> : ISpecification<T>
    {
        // Lista de criterios para soportar múltiples condiciones
        private List<Expression<Func<T, bool>>> _criteria = new List<Expression<Func<T, bool>>>();

        public Specification()
        {
        }

        public Specification(Expression<Func<T, bool>> criteria)
        {
            _criteria.Add(criteria);
        }

        // Propiedad para acceder a los criterios combinados
        public Expression<Func<T, bool>> Criteria
        {
            get
            {
                // Combinar todos los criterios con AND
                return _criteria.Aggregate((current, next) =>
                    current == null ? next : current.And(next));
            }
        }

        // Método para agregar criterios adicionales
        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            _criteria.Add(criteria);
        }

        // Resto de tu implementación actual...
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;

        public int Take { get; private set; }
        public int Skip { get; private set; }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
        protected void AddInclude(string includeString) => IncludeStrings.Add(includeString);
    }
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var visitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftVisited = visitor.Visit(left.Body);
            visitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightVisited = visitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(leftVisited, rightVisited), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                return node == _oldValue ? _newValue : base.Visit(node);
            }
        }
    }*/
    /*public class Specification<T> : ISpecification<T>
    {
        public Specification()
        {
        }
        public Specification(Expression<Func<T, bool>> criteria) => Criteria = criteria;

        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression) => OrderBy = orderByExpression;
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression) => OrderByDescending = orderByDescendingExpression;

        public int Take { get; private set; }
        public int Skip { get; private set; }

       
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        public bool IsPagingEnabled { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);
        protected void AddInclude(string includeString) => IncludeStrings.Add(includeString);
    }*/
}
