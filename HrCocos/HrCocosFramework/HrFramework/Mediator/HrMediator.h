#ifndef _HR_MEDIATOR_H_
#define _HR_MEDIATOR_H_

#include "HrFramework/HrFrameworkPrerequisite.h"

namespace Hr
{
	class HrMediator
	{
	public:
		HrMediator();
		virtual ~HrMediator();

		void BindView(HrView* pView);

	protected:
		virtual void OnRegister();

	protected:
		HrView* m_pView;
	};
}


#endif



