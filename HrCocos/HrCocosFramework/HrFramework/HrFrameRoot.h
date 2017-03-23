#ifndef _HR_FRAMEROOT_H_
#define _HR_FRAMEROOT_H_

#include "HrFrameworkPrerequisite.h"
#include "HrSingleton.h"
#include "Mediator/HrMediator.h"

namespace Hr
{
	class HrFrameRoot
	{
	public:
		HrFrameRoot();
		~HrFrameRoot();

		virtual void Init();
		virtual void Start();
		virtual void Stop();

		template <typename T>
		void BindViewToMediator(HrView* pView);
	protected:
		HrContextPtr m_pContext;
	};

	template <typename T>
	void HrFrameRoot::BindViewToMediator(HrView* pView)
	{
		auto pMediator = std::make_shared<T>();
		pMediator->BindView(pView);
	}

}


#endif



