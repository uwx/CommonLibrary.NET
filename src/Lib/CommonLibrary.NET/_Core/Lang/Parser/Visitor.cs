using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// Class that visits each ast node in the trees.
    /// </summary>
    public class AstVisitor
    {
        /// <summary>
        /// Callback 
        /// </summary>
        private Action<AstNode> _callBack;


        /// <summary>
        /// Initialize
        /// </summary>
        public AstVisitor()
        {
            _callBack = null;
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public AstVisitor(Action<AstNode> callBack)
        {
            _callBack = callBack;
        }


        /// <summary>
        /// Visits each statement
        /// </summary>
        /// <param name="stmts"></param>
        public void Visit(List<Stmt> stmts)
        {
            foreach (var stmt in stmts)
            {
                Visit(stmt);
            }
        }


        /// <summary>
        /// Visit the statement
        /// </summary>
        /// <param name="stmt"></param>
        public void Visit( Stmt stmt)
        {
            if (stmt is AssignStmt)
                Var(stmt as AssignStmt);

            else if (stmt is ForStmt)
                For(stmt as ForStmt);

            else if (stmt is ForEachStmt)
                ForEach(stmt as ForEachStmt);
            
            else if (stmt is IfStmt)
                If(stmt as IfStmt);            
            
            else if (stmt is TryCatchStmt)
                Try(stmt as TryCatchStmt);

            else if (stmt is WhileStmt)
                While(stmt as WhileStmt);
        }


        /// <summary>
        /// Used to visit an expression.
        /// </summary>
        /// <param name="exp">expression</param>
        public void Visit(Expr exp)
        {
            if (exp is BinaryExpr)
                Binary(exp as BinaryExpr);

            else if (exp is CompareExpr)
                Compare(exp as CompareExpr);

            else if (exp is ConditionExpr)
                Condition(exp as ConditionExpr);

            else if (exp is FunctionCallExpr)
                FunctionCall(exp as FunctionCallExpr);
        }


        /// <summary>
        /// Visits the var statement tree.
        /// </summary>
        /// <param name="assignStmt"></param>
        public void Var(AssignStmt assignStmt)
        {
            _callBack(assignStmt);
            foreach (var decl in assignStmt._declarations)
            {
                Visit(decl.Item1);
                Visit(decl.Item2);
            }
        }


        /// <summary>
        /// Visits the for statement tree.
        /// </summary>
        /// <param name="forStmt"></param>
        public void For(ForStmt forStmt)
        {
            _callBack(forStmt);
            Visit(forStmt.Start);
            Visit(forStmt.Condition);
            Visit(forStmt.Increment);
            foreach (var stmt in forStmt.Statements)
            {
                Visit(stmt);
            }
        }


        /// <summary>
        /// Visits the for each statement tree.
        /// </summary>
        /// <param name="forStmt"></param>
        public void ForEach(ForEachStmt forStmt)
        {
            _callBack(forStmt);
            Visit(forStmt.Condition);
            foreach (var stmt in forStmt.Statements)
            {
                Visit(stmt);
            }
        }


        /// <summary>
        /// Visits the if statement tree.
        /// </summary>
        /// <param name="ifStmt"></param>
        public void If(IfStmt ifStmt)
        {
            _callBack(ifStmt);
            Visit(ifStmt.Condition);
            foreach (var stmt in ifStmt.Statements)
            {
                Visit(stmt);
            }
            Visit(ifStmt.Else);
        }


        /// <summary>
        /// Visits the try statement tree.
        /// </summary>
        /// <param name="tryStmt"></param>
        public void Try(TryCatchStmt tryStmt)
        {
            _callBack(tryStmt);
            foreach (var stmt in tryStmt.Statements)
            {
                Visit(stmt);
            }
            Visit(tryStmt.Catch);
        }


        /// <summary>
        /// Visits the while statement tree.
        /// </summary>
        /// <param name="whileStmt"></param>
        public void While(WhileStmt whileStmt)
        {
            _callBack(whileStmt);
            Visit(whileStmt.Condition);
            foreach (var stmt in whileStmt.Statements)
            {
                Visit(stmt);
            }
        }


        /// <summary>
        /// Visits the binary expression tree
        /// </summary>
        /// <param name="exp"></param>
        public void Binary(BinaryExpr exp)
        {
            _callBack(exp);
            _callBack(exp.Left);
            _callBack(exp.Right);
        }


        /// <summary>
        /// Visits the compare expression tree
        /// </summary>
        /// <param name="exp"></param>
        public void Compare(CompareExpr exp)
        {
            _callBack(exp);
            _callBack(exp.Left);
            _callBack(exp.Right);
        }


        /// <summary>
        /// Visits the condition expression tree
        /// </summary>
        /// <param name="exp"></param>
        public void Condition(ConditionExpr exp)
        {
            _callBack(exp);
            _callBack(exp.Left);
            _callBack(exp.Right);
        }


        /// <summary>
        /// Visits the function call expression tree
        /// </summary>
        /// <param name="exp"></param>
        public void FunctionCall(FunctionCallExpr exp)
        {
            _callBack(exp);
            foreach (var paramExp in exp.ParamListExpressions)
                _callBack(paramExp);
        }
    }
}
