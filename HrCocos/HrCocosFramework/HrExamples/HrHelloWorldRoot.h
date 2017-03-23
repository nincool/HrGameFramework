#ifndef _HR_HELLOWORLDROOT_H_
#define _HR_HELLOWORLDROOT_H_

#include "HrFramework/HrFrame.h"

namespace Hr
{
	class HrHelloWorldRoot : public HrFrameRoot, public HrSingleTon<HrHelloWorldRoot>
	{
	public:
		HrHelloWorldRoot();
		~HrHelloWorldRoot();

		virtual void Init() override;
		virtual void Start() override;
	};
}


#endif



