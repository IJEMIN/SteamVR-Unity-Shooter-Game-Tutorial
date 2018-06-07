using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public LayerMask m_TargetLayer; // 폭발 감지를 적용할 대상의 필터

    public ParticleSystem m_ExplosionEffectPrafab; // 폭발할 순간에 생성할 이펙트의 원본 프리팹
    
    public float m_Damage = 100; // 데미지의 양
    public float m_ExplosionRadius = 5f; // 폭발 반경
    public float m_TimeToExplode = 5f; // 폭발에 걸리는 시간

    private bool m_Cooking = false; // 폭발 카운트 다운이 시작되었는가 

    // 수류탄을 쿠킹 (카운트 다운 시작)
    public void CookGrenade()
    {
        // 수류탄을 쿠킹 중이라면, 처리를 종료
        if(m_Cooking)
        {
            return;
        }

        m_Cooking = true;
        // 지연시간뒤에 Explode를 실행
        Invoke("Explode",m_TimeToExplode);
    }

    // 실제 폭발 처리를 하는 부분
    private void Explode()
    {
        // transform.position을 중심으로 m_ExplosionRadius 만큼 반지름을 가진 구(공)을 그려서,
        // 거기에 겹치는 모든 충돌체들을 가져옴
        // 필터링을 추가하여 m_TargetLayer 에 해당되는 충돌체만 감지
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TargetLayer);

        // 감지된 콜라이더들을 통해 상대방이 IDamageable을 가지고 있다면 데미지를 실제로 주는 처리
        for(int i = 0; i< colliders.Length; i++)
        {
            IDamageable target = colliders[i].GetComponent<IDamageable>();

            if(target != null)
            {
                target.OnDamage(m_Damage);
            }
        }

        // 파티클 효과를 생성 후 재생

        ParticleSystem explosionEffect = Instantiate(m_ExplosionEffectPrafab, transform.position, transform.rotation);
        explosionEffect.Play();

        // 파티클 효과가 재생이 전부되었을때, 파괴
        Destroy(explosionEffect.gameObject, explosionEffect.main.duration);

        // 수류탄 자기 자신을 파괴
        Destroy(gameObject);
    }


}
