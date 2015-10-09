using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Qujck.MarkdownEditor.Infrastructure
{
    // http://blog.tomasm.net/2009/11/07/forwarding-meta-object/
    public sealed class ForwardingMetaObject : DynamicMetaObject
    {
        private readonly DynamicMetaObject metaForwardee;

        public ForwardingMetaObject(
            Expression expression,
            BindingRestrictions restrictions,
            object forwarder,
            IDynamicMetaObjectProvider forwardee,
            Func<Expression, Expression> forwardeeGetter
            ) : base(expression, restrictions, forwarder)
        {
            // We'll use forwardee's meta-object to bind dynamic operations.
            metaForwardee = forwardee.GetMetaObject(
                forwardeeGetter(
                    Expression.Convert(expression, forwarder.GetType())   // [1]
                )
            );
        }

        // Restricts the target object's type to TForwarder. 
        // The meta-object we are forwarding to assumes that it gets an instance of TForwarder (see [1]).
        // We need to ensure that the assumption holds.
        private DynamicMetaObject AddRestrictions(DynamicMetaObject result)
        {
            var restricted = new DynamicMetaObject(
                result.Expression,
                BindingRestrictions.GetTypeRestriction(Expression, Value.GetType()).Merge(result.Restrictions),
                this.metaForwardee.Value);
            return restricted;
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
        {
            return AddRestrictions(this.metaForwardee.BindGetMember(binder));
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
        {
            return AddRestrictions(this.metaForwardee.BindSetMember(binder, value));
        }

        public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
        {
            return AddRestrictions(this.metaForwardee.BindDeleteMember(binder));
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
        {
            return AddRestrictions(this.metaForwardee.BindGetIndex(binder, indexes));
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
        {
            return AddRestrictions(this.metaForwardee.BindSetIndex(binder, indexes, value));
        }

        public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
        {
            return AddRestrictions(this.metaForwardee.BindDeleteIndex(binder, indexes));
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            return AddRestrictions(this.metaForwardee.BindInvokeMember(binder, args));
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            return AddRestrictions(this.metaForwardee.BindInvoke(binder, args));
        }

        public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
        {
            return AddRestrictions(this.metaForwardee.BindCreateInstance(binder, args));
        }

        public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
        {
            return AddRestrictions(this.metaForwardee.BindUnaryOperation(binder));
        }

        public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
        {
            return AddRestrictions(this.metaForwardee.BindBinaryOperation(binder, arg));
        }

        public override DynamicMetaObject BindConvert(ConvertBinder binder)
        {
            return AddRestrictions(this.metaForwardee.BindConvert(binder));
        }
    }
}
