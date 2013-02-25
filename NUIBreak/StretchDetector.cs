using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.IO;

using Microsoft.Kinect;
using Kinect.Toolbox;

namespace NUIBreak
{
    class StretchDetector : PostureDetector
    {

        public Dictionary<string, StretchesStretch> AvailableStretches = new Dictionary<string,StretchesStretch>();
        public string CurrentStretch;

        public StretchDetector() : base(10)
        {
            using (StreamReader reader = new StreamReader(@"Stretches\stretches.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stretches));
                Stretches stretches = (Stretches)serializer.Deserialize(reader);

                foreach (StretchesStretch stretch in stretches.Stretch)
                {
                    AvailableStretches.Add(stretch.Name, stretch);
                }
            }
        }

        public override void TrackPostures(Skeleton skeleton)
        {
            if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                return;

            StretchesStretch current = AvailableStretches[CurrentStretch];

            bool detected = true;
            foreach (StretchesStretchRule rule in current.Rule)
            {
                if ( !detected ) 
                {
                    break;
                }
                RuleEvaluation eval;
                switch (rule.Type)
                {
                    case StretchesStretchRuleType.Compare:
                        eval = new CompareRuleEvaluation(skeleton, rule);
                        detected = eval.Evaluate();
                        break;
                    case StretchesStretchRuleType.Distance:
                        eval = new DistanceRuleEvaluation(skeleton, rule);
                        detected = eval.Evaluate();
                        break;
                    default:
                        break;
                }
            }

            if ( detected )
            {
                RaisePostureDetected(CurrentStretch);
                return;
            }

            Reset();
        }
    }

    abstract class RuleEvaluation
    {
        protected StretchesStretchRule Rule;
        protected Skeleton CurrentSkeleton;
        protected Vector3 Joint1, Joint2;
        protected float Joint1Position = 0, Joint2Position = 0;
        protected float Value1 = 0, Value2 = 0;

        public RuleEvaluation(Skeleton skeleton, StretchesStretchRule rule)
        {
            Rule = rule;
            CurrentSkeleton = skeleton;
            Joint1 = GetJointPosition(skeleton, rule.Joint1);
            Joint2 = GetJointPosition(skeleton, rule.Joint2);

            CalculateJointPositions();
            CalculateComparisonValues();
        }

        protected virtual void CalculateJointPositions()
        {
            switch (Rule.Axis)
            {
                case StretchesStretchRuleAxis.X:
                    Joint1Position = Joint1.X;
                    Joint2Position = Joint2.X;
                    break;
                case StretchesStretchRuleAxis.Y:
                    Joint1Position = Joint1.Y;
                    Joint2Position = Joint2.Y;
                    break;
                case StretchesStretchRuleAxis.Z:
                    Joint1Position = Joint1.Z;
                    Joint2Position = Joint2.Z;
                    break;
                case StretchesStretchRuleAxis.All:
                    break;
                default:
                    break;
            }
        }

        protected Vector3 GetJointPosition(Skeleton skeleton, string joint)
        {
            return skeleton.Joints[(JointType)Enum.Parse(typeof(JointType), joint)].Position.ToVector3();
        }

        protected abstract void CalculateComparisonValues();

        public virtual bool Evaluate()
        {
            switch (Rule.Operator)
            {
                case StretchesStretchRuleOperator.EQ:
                    return (Value1 == Value2);
                case StretchesStretchRuleOperator.GT:
                    return (Value1 > Value2);
                case StretchesStretchRuleOperator.LT:
                    return (Value1 < Value2);
                default:
                    return false;
            }
        }
    }

    class CompareRuleEvaluation : RuleEvaluation
    {
        public CompareRuleEvaluation(Skeleton skeleton, StretchesStretchRule rule) : base(skeleton, rule) { }

        protected override void CalculateComparisonValues()
        {
            Value1 = Joint1Position;
            Value2 = Joint2Position;
        }
    }

    class DistanceRuleEvaluation : RuleEvaluation
    {
        public DistanceRuleEvaluation(Skeleton skeleton, StretchesStretchRule rule) : base(skeleton, rule) { }

        protected override void CalculateComparisonValues()
        {
            if (Rule.Axis == StretchesStretchRuleAxis.All)
            {
                Value1 = (Joint1 - Joint2).Length;
            }
            else
            {
                Value1 = Math.Abs(Joint1Position - Joint2Position);
            }

            Value2 = Rule.Range;
        }
    }
}
