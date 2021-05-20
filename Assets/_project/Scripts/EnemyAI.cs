using Project.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Project.Units {
    public class EnemyAI : MonoBehaviour {
        private Transform _myTarget;
        private NavMeshAgent _navMeshAgent;
        private Unit _unit;

        private void Awake() {
            _unit = GetComponent<Unit>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start() {
            GetRandomTargetFromHolder();
            MoveToTarget();
        }

        private void GetRandomTargetFromHolder() {
            _myTarget = UnitsHolderManager.instance.GetHostileUnitsTransformsByUnitId(_unit.fractionIdentifier);
        }


        void MoveToTarget() {
            if(_myTarget == null)
                return;
            _navMeshAgent.SetDestination(_myTarget.position);
        }
        
        //Find Target
            //обратиться к ЮнитХолдеру(в старте), попросить у него список врагов и подписаться на обновление списка (при обновлении списка должен передаваться Id фракции)
            // зачекать список, выбрать цель
        //FindRoot to target 
            //разобраться с AI Navigator, проложить путь до цели
        //Travel
            //просто двигать туда, куда говорит навигатор
        //Shoot the Target till his death
            // добавить missOffset, чтобы не стрелял чётко в цель,также в зависимости от оружия время нажатия на спуск после которого отжатие
        //joggle
            //двигаться влево вправо разными интервалами (и скоростью) имитировать попытку увернутсья от пуль врага
        //???
        //repeat
            // таргет, что перед тобой кончился? зачекай список, выбере одногои продолжай движение(собственно первый пункт)
        //no target? Chill and smoke
            // если целей нет, жди когда произойдёт обновление списка
            
            
        
    }
}
