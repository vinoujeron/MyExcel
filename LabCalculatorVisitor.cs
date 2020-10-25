using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Laba1
{
    class CalcVisitor : CalcBaseVisitor<double>
    {
        public override double VisitCompileUnit(CalcParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }
        public override double VisitNumberExpr(CalcParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            Debug.WriteLine(result);
            return result;
        }
        public override double VisitIdentifierExpr(CalcParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            //Form1.GetInstance().Fooo(Form1.GetInstance().CellNameToCellValue(result));
            if (Form1.GetInstance().ContainsKey(result)) {
                
                return Double.Parse((Form1.GetInstance().CellNameToCellValue(result)));
            }
            else
            {
                return Double.Parse("NOT A DOUBLE");
            }
        }

        public override double VisitMaxMinExpr(CalcParser.MaxMinExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            if (context.operatorToken.Type == CalcLexer.MAX)
            {
                return Math.Max(left, right);
            }
            else
                return Math.Min(left, right);
        }

        public override double VisitMmin(CalcParser.MminContext context)
        {
            double minValue = Double.PositiveInfinity;

            foreach (var child in context.paramlist.children.OfType<CalcParser.ExpressionContext>())
            {
                double childValue = this.Visit(child);

                if (childValue < minValue)
                {
                    minValue = childValue;
                }
            }

            return minValue;

        }
        
        public override double VisitUnaryMinus(CalcParser.UnaryMinusContext context)
        {
            return -1 * WalkLeft(context);
        }

        public override double VisitUnaryPlus(CalcParser.UnaryPlusContext context)
        {
            return WalkLeft(context);
        }
        
        public override double VisitModExpr(CalcParser.ModExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            return left % right;
        }

        public override double VisitDespExpr(CalcParser.DespExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);

            return left + (right / 10);
        }
        
        public override double VisitMmax(CalcParser.MmaxContext context)
        {
            double maxValue = Double.NegativeInfinity;

            foreach (var child in context.paramlist.children.OfType<CalcParser.ExpressionContext>())
            {
                double childValue = this.Visit(child);

                if (childValue > maxValue)
                {
                    maxValue = childValue;
                }
            }

            return maxValue;

        }

        public override double VisitParenthesizedExpr(CalcParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }
        public override double VisitExponentialExpr(CalcParser.ExponentialExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            Debug.WriteLine("{0} ^ {1}", left, right);
            return System.Math.Pow(left, right);
        }
        public override double VisitAdditiveExpr(CalcParser.AdditiveExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == CalcLexer.ADD)
            {
                Debug.WriteLine("{0} + {1}", left, right);
                return left + right;
            }
            else 
            {
                Debug.WriteLine("{0} - {1}", left, right);
                return left - right;
            }
        }
        
        public override double VisitIncExpr(CalcParser.IncExprContext context)
        {
            var left = WalkLeft(context);
            return ++left;
        }
        public override double VisitDecExpr(CalcParser.DecExprContext context)
        {
            var left = WalkLeft(context);
            return --left;
        }
        public override double VisitMultiplicativeExpr(CalcParser.MultiplicativeExprContext context)
        {
            var left = WalkLeft(context);
            var right = WalkRight(context);
            if (context.operatorToken.Type == CalcLexer.MULTIPLY)
            {
                Debug.WriteLine("{0} * {1}", left, right);
                return left * right;
            }
            else 
            {
                Debug.WriteLine("{0} / {1}", left, right);
                return left / right;
            }
        }
        private double WalkLeft(CalcParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<CalcParser.ExpressionContext>(0));
        }
        private double WalkRight(CalcParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<CalcParser.ExpressionContext>(1));
        }
    }
}